using System.Net;
using Application.Configurations;
using Application.Data;
using Application.Data.Enums;
using Application.Data.Validation;
using Application.Data.ViewModel;
using Application.Resources;
using CrossCutting.Helpers;
using Domain;
using Domain.Model;
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
            {
                var error = new Dictionary<string, IEnumerable<string>>
                {
                    { "ID", [_localizer["InvalidId"].Value] }
                };
                throw new AppException(_localizer["InvalidId"].Value, error);
            }

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
            var validator = new CreateSerieValidation(_localizer, _context);
            var validation = await validator.ValidateAsync(dto);
            if (!validation.IsValid)
                throw new AppException(_localizer["OneOrMoreValidationErrorsOccurred"], validation.Errors);

            var errors = new Dictionary<string, IEnumerable<string>>();

            var checkResourceAlreadyExists = false;

            var newResources = new List<string>
            {
                dto.SynopsisResource,
                dto.TitleResource
            };

            List<string> messages = [];
            foreach (var se in dto.Seasons)
            {
                foreach (var ep in se.Episodes)
                {
                    checkResourceAlreadyExists = newResources
                        .Where(m => m.Equals(ep.SynopsisResource) || m.Equals(ep.SynopsisResource))
                        .Any();

                    if (checkResourceAlreadyExists)
                        messages.Add($"SynopsisResource: {_localizer["AlreadyExists"].Value}");

                    checkResourceAlreadyExists = newResources
                        .Where(m => m.Equals(ep.TitleResource) || m.Equals(ep.TitleResource))
                        .Any();

                    if (checkResourceAlreadyExists)
                        messages.Add($"TitleResource: {_localizer["AlreadyExists"].Value}");

                    if (messages.Any())
                    {
                        errors.Add($"Episode {se.Number:00} x {ep.Number:00}", messages);
                        messages.Clear();
                    }

                    newResources.AddRange([ep.SynopsisResource, ep.TitleResource]);
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
                TitleResource = dto.TitleResource,
                TimelineId = (byte)dto.TimelineId.GetHashCode(),
                TmdbId = dto.TmdbId,
                Seasons = []
            };

            Parallel.ForEach(dto.Seasons, new ParallelOptions(), s =>
            {
                var newSeason = new Season()
                {
                    Number = s.Number,
                    Episodes = []
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

            var serieSaved = await _context.Serie.AsNoTracking()
                .Include(s => s.Seasons).ThenInclude(s => s.Episodes)
                .Include(s => s.Language)
                .OrderBy(s => s.SerieId)
                .LastAsync();

            return new SerieVM()
            {
                ID = serieSaved.SerieId,
                Abbreviation = serieSaved.Abbreviation,
                ImdbId = serieSaved.ImdbId,
                OriginalLanguage = serieSaved.Language.CodeISO,
                OriginalName = serieSaved.OriginalName,
                Seasons = serieSaved.Seasons.Select(se => new SeasonVM(se.SeasonId, se.Number, se.Episodes.ToList())).ToList(),
                Timeline = serieSaved.TimelineId,
                NameTranslated = _titleSynopsisLocalizer[serieSaved.TitleResource],
                Synopsis = _titleSynopsisLocalizer[serieSaved.SynopsisResource]
            };
        }

        public async Task UpdateSerieById(byte id, UpdateSerieDto dto)
        {
            var dtoValidation = new UpdateSerieValidation(_localizer);
            var validation = dtoValidation.Validate(dto);
            if (!validation.IsValid)
                throw new AppException(_localizer["OneOrMoreValidationErrorsOccurred"], validation.Errors);

            var serie = await _context.Serie.Where(s => s.SerieId.Equals(id)).FirstOrDefaultAsync()
                ?? throw new AppException(_localizer["NotFound"].Value, HttpStatusCode.NotFound);

            serie.Abbreviation = string.IsNullOrWhiteSpace(dto.Abbreviation) ? serie.Abbreviation : dto.Abbreviation;
            serie.ImdbId = string.IsNullOrWhiteSpace(dto.ImdbId) ? serie.ImdbId : dto.ImdbId.Trim();
            serie.OriginalName = string.IsNullOrWhiteSpace(dto.OriginalName) ? serie.OriginalName : dto.OriginalName.Trim();
            serie.TimelineId = (byte?)dto.TimelineId?.GetHashCode() ?? serie.TimelineId;
            serie.TmdbId = dto.TmdbId ?? serie.TmdbId;

            await _context.SaveChangesAsync();
        }
    }
}
