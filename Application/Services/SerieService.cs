using System.Collections.ObjectModel;
using System.Net;
using Application.Configurations;
using Application.Data;
using Application.Data.Enum;
using Application.Data.Validation;
using Application.Data.ViewModel;
using Application.Helpers;
using Application.Model;
using Application.Resources;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Application.Services
{
    public class SerieService(StarTrekContext context,
        IStringLocalizer<Messages> localizer,
        IStringLocalizer<TitleSynopsis> titleSynopsisLocalizer)
    {
        private readonly StarTrekContext _context = context;
        private readonly IStringLocalizer<Messages> _localizer = localizer;
        private readonly IStringLocalizer<TitleSynopsis> _titleSynopsisLocalizer = titleSynopsisLocalizer;

        public async Task<IEnumerable<SerieVM>> GetSeriesList(byte page, byte pageSize)
        {
            pageSize = pageSize == 0 ? (byte)100 : pageSize;

            var seriesList = await _context.Serie
                .AsNoTracking()
                .AsSplitQuery()
                .OrderBy(s => s.SerieId)
                .Skip(page * pageSize)
                .Take(pageSize)
                .Select(s => new SerieVM
                {
                    ID = s.SerieId,
                    OriginalName = s.OriginalName,
                    OriginalLanguage = s.Language.CodeISO,
                    ImdbId = s.ImdbId,
                    Abbreviation = s.Abbreviation,
                    Seasons = s.Seasons.Select(se => new SeasonVM(se.SeasonId, se.Number, se.Episodes)).ToArray(),
                    Timeline = s.TimelineId,
                    NameTranslated = _titleSynopsisLocalizer[s.TitleResource].Value,
                    Synopsis = _titleSynopsisLocalizer[s.SynopsisResource].Value
                }).ToArrayAsync();

            if (seriesList.Any())
                return seriesList;

            throw new AppException(_localizer["NotFound"].Value, HttpStatusCode.NotFound);
        }

        public async Task<SerieVM> GetSerieById(byte serieId)
        {
            if (serieId <= 0)
                throw new ArgumentException(_localizer["InvalidId"].Value);

            return await _context.Serie
                .AsNoTracking()
                .Where(s => s.SerieId == serieId)
                .Select(s => new SerieVM
                {
                    ID = s.SerieId,
                    OriginalName = s.OriginalName,
                    OriginalLanguage = s.Language.CodeISO,
                    ImdbId = s.ImdbId,
                    Abbreviation = s.Abbreviation,
                    Seasons = s.Seasons.Select(se => new SeasonVM(se.SeasonId, se.Number, se.Episodes)).ToArray(),
                    NameTranslated = _titleSynopsisLocalizer[s.TitleResource].Value,
                    Synopsis = _titleSynopsisLocalizer[s.SynopsisResource].Value,
                    Timeline = s.TimelineId
                })
                .FirstOrDefaultAsync()
                ?? throw new AppException(_localizer["NotFound"].Value, HttpStatusCode.NotFound);
        }

        public async Task<SerieVM> CreateNewSerie(CreateSerieDto dto)
        {
            var dtoValidation = new CreateSerieValidation(_localizer);
            var validation = dtoValidation.Validate(dto);
            if (!validation.IsValid)
                throw new AppException(_localizer["OneOrMoreValidationErrorsOccurred"], validation.Errors);

            var checkExists = _context.Serie.AsNoTracking()
                .Where(s => s.ImdbId.Equals(dto.ImdbId) || s.TmdbId.Equals(dto.TmdbId))
                .ToArrayAsync();

            var errors = new Dictionary<string, IEnumerable<string>>();

            if (checkExists.Result.Any())
            {
                errors.Add("ImdbId/TmdbId", [_localizer["ImdbOrTmdbIdAlreadyRegistered"].Value]);
                throw new AppException(_localizer["NotCreated"].Value, errors);
            }

            var registeredResources = await _context.vwResourcesName.AsNoTracking().ToListAsync();
            var checkResourceAlreadyExists = registeredResources
                .Where(m => m.SynopsisResource.Equals(dto.SynopsisResource)
                    || m.TitleResource.Equals(dto.SynopsisResource))
                .Any();

            if (checkResourceAlreadyExists)
                errors.Add("Serie: SynopsisResource", [_localizer["AlreadyExists"].Value]);

            if (!string.IsNullOrWhiteSpace(dto.TitleResource))
            {
                checkResourceAlreadyExists = registeredResources
                    .Where(m => m.TitleResource.Equals(dto.TitleResource)
                        || m.SynopsisResource.Equals(dto.TitleResource))
                    .Any();
            }

            if (checkResourceAlreadyExists)
                errors.Add("Serie: TitleResource", [_localizer["AlreadyExists"].Value]);

            registeredResources.Add(new vwResourcesName(dto.SynopsisResource, dto.TitleResource));

            bool checkSynopsisResourceAlreadyExists = false;
            bool checkTitleResourceAlreadyExists = false;
            List<string> messages = [];
            foreach (var se in dto.Seasons)
            {
                foreach(var ep in se.Episodes)
                {
                    checkSynopsisResourceAlreadyExists = registeredResources
                        .Where(m => m.SynopsisResource.Equals(dto.SynopsisResource)
                            || m.TitleResource.Equals(dto.SynopsisResource))
                        .Any();

                    if (checkResourceAlreadyExists)
                        messages.Add($"SynopsisResource: {_localizer["AlreadyExists"].Value}");

                    checkTitleResourceAlreadyExists = registeredResources
                        .Where(m => m.TitleResource.Equals(dto.TitleResource)
                            || m.SynopsisResource.Equals(dto.TitleResource))
                        .Any();

                    if (checkResourceAlreadyExists)
                        messages.Add($"TitleResource: {_localizer["AlreadyExists"].Value}");

                    if (messages.Any())
                    {
                        errors.Add($"Episode {se.Number:00} x {ep.Number:00}", messages);
                        messages.Clear();
                    }

                    registeredResources.Add(new vwResourcesName(ep.SynopsisResource, ep.TitleResource));
                }
            }

            if (errors.Any())
                throw new AppException(_localizer["NotCreated"].Value, errors);

            var languageIso = RegexHelper.RemoveSpecialCharacters(dto.OriginalLanguageIso);

            var newSerie = new Serie()
            {
                Abbreviation = dto.Abbreviation,
                ImdbId = dto.ImdbId,
                OriginalLanguageId = (short)Enum.Parse<LanguageEnum>(languageIso, true).GetHashCode(),
                OriginalName = dto.OriginalName.Trim(),
                SynopsisResource = dto.SynopsisResource,
                TimelineId = (byte)dto.TimelineId.GetHashCode(),
                TmdbId = dto.TmdbId,
                Seasons = new Collection<Season>()
            };

            Parallel.ForEach(dto.Seasons, new ParallelOptions(), s =>
            {
                var newSeason = new Season()
                {
                    Number = s.Number,
                    Episodes = new Collection<Episode>()
                };

                Parallel.ForEach(s.Episodes, new ParallelOptions(), episode =>
                {
                    newSeason.Episodes.Add(new Episode
                    {
                        ImdbId = episode.ImdbId,
                        Number = episode.Number,
                        RealeaseDate = episode.RealeaseDate,
                        StardateFrom = episode.StardateFrom,
                        StardateTo = episode.StardateTo,
                        SynopsisResource = episode.SynopsisResource,
                        Time = episode.Time,
                        TitleResource = episode.TitleResource
                    });
                });
                newSerie.Seasons.Add(newSeason);
            });

            await _context.Serie.AddAsync(newSerie);
            await _context.SaveChangesAsync();

            var serieToReturn = await _context.Serie.AsNoTracking()
                .Include(s => s.Seasons).ThenInclude(s => s.Episodes)
                .Include(s => s.Language)
                .OrderBy(s => s.SerieId)
                .Select(s => new SerieVM()
                {
                    ID = s.SerieId,
                    Abbreviation = s.Abbreviation,
                    ImdbId = s.ImdbId,
                    OriginalLanguage = s.Language.CodeISO,
                    OriginalName = s.OriginalName,
                    Seasons = s.Seasons.Select(se => new SeasonVM(se.SeasonId, se.Number, se.Episodes.ToArray())).ToArray(),
                    Timeline = s.TimelineId,
                    NameTranslated = _titleSynopsisLocalizer[s.TitleResource] ?? _localizer["NotFound"].Value,
                    Synopsis = _titleSynopsisLocalizer[s.SynopsisResource] ?? _localizer["NotFound"].Value
                })
                .LastAsync();

            return serieToReturn;
        }

        public async Task UpdateSerieById(byte id, UpdateSerieDto dto)
        {
            var dtoValidation = new UpdateSerieValidation(_localizer);
            var validation = dtoValidation.Validate(dto);
            if (!validation.IsValid)
                throw new AppException(_localizer["OneOrMoreValidationErrorsOccurred"], validation.Errors);

            var serie = await _context.Serie.Where(s => s.SerieId.Equals(id)).FirstOrDefaultAsync()
                ?? throw new AppException(_localizer["NotFound"].Value, HttpStatusCode.NotFound);
            
            var errors = new Dictionary<string, IEnumerable<string>>();

            var registeredResources = await _context.vwResourcesName.AsNoTracking().ToArrayAsync();
            var alreadyExistsResource = registeredResources.Where(r =>
                    !r.Id.Equals($"s{id}")
                    && (r.SynopsisResource.Equals(dto.SynopsisResource) || r.TitleResource.Equals(dto.SynopsisResource))
                ).Any();
            
            if (alreadyExistsResource)
                errors.Add("SynopsisResource", [_localizer["AlreadyExists"].Value]);

            alreadyExistsResource = registeredResources.Where(r =>
                    !r.Id.Equals($"s{id}")
                    && (r.SynopsisResource.Equals(dto.TitleResource) || r.TitleResource.Equals(dto.TitleResource))
                ).Any();

            if (alreadyExistsResource)
                errors.Add("TitleResource", [_localizer["AlreadyExists"].Value]);

            if (errors.Any())
                throw new AppException(_localizer["Error"].Value, errors);

            serie.Abbreviation = string.IsNullOrWhiteSpace(dto.Abbreviation) ? serie.Abbreviation : dto.Abbreviation;
            serie.ImdbId = string.IsNullOrWhiteSpace(dto.ImdbId) ? serie.ImdbId : dto.ImdbId.Trim();
            serie.OriginalName = string.IsNullOrWhiteSpace(dto.OriginalName) ? serie.OriginalName : dto.OriginalName.Trim();
            serie.SynopsisResource = string.IsNullOrWhiteSpace(dto.SynopsisResource) ? serie.SynopsisResource : dto.SynopsisResource;
            serie.TimelineId = (byte?)dto.TimelineId?.GetHashCode() ?? serie.TimelineId;
            serie.TitleResource = string.IsNullOrWhiteSpace(dto.TitleResource) ? serie.TitleResource : dto.TitleResource;
            serie.TmdbId = dto.TmdbId ?? serie.TmdbId;

            await _context.SaveChangesAsync();
        }
    }
}
