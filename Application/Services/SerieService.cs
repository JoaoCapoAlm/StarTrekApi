using System.Collections.Generic;
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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using static Application.Middleware.AppMiddleware;

namespace Application.Services
{
    public class SerieService
    {
        private readonly StarTrekContext _context;
        private readonly IStringLocalizer<Messages> _localizer;
        public SerieService(StarTrekContext context, IStringLocalizer<Messages> localizer)
        {
            _context = context;
            _localizer = localizer;
        }

        public async Task<IEnumerable<SerieVM>> GetSeriesList(byte page, byte pageSize)
        {
            pageSize = pageSize == 0 ? (byte)100 : pageSize;

            var seriesList = await _context.Serie
                .AsNoTracking()
                .OrderBy(s => s.SerieId)
                .Skip(page * pageSize)
                .Take(pageSize)
                .Select(s => new SerieVM
                {
                    ID = s.SerieId,
                    OriginalName = s.OriginalName,
                    OriginalLanguage = s.Language.ResourceName,
                    ImdbId = s.ImdbId,
                    Abbreviation = s.Abbreviation,
                    Seasons = s.Seasons.Select(se => new SeasonVM(se.SeasonId, se.Number, se.Episodes)).ToArray(),
                    Timeline = s.TimelineId
                })
                .ToArrayAsync();

            if (seriesList.Any())
                return seriesList;
            
            throw new AppException(_localizer["NotFound"].Value, Enumerable.Empty<ErrorContent>(), System.Net.HttpStatusCode.NotFound);
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
                    Seasons = s.Seasons.Select(se => new SeasonVM(se.SeasonId, se.Number, se.Episodes)).ToArray()
                })
                .FirstOrDefaultAsync()
                ?? throw new AppException(_localizer["NotFound"].Value, Enumerable.Empty<ErrorContent>(), HttpStatusCode.NotFound);
        }

        public async Task<SerieVM> CreateNewSerie(CreateSerieDto dto)
        {
            var checkExists = await _context.Serie.AsNoTracking()
                .Where(s => s.ImdbId.Equals(dto.ImdbId) || s.TmdbId.Equals(dto.TmdbId))
                .ToListAsync();
            if (checkExists.Any())
            {
                var listError = new List<ErrorContent>()
                {
                    new("ImdbId/TmdbId", _localizer["ImdbOrTmdbIdAlreadyRegistered"].Value)
                };
                throw new AppException(_localizer["NotCreated"].Value, listError);
            }

            var errors = new List<ErrorContent>();
            var registeredResources = await _context.vwResourcesName.AsNoTracking().ToArrayAsync();
            var checkResourceAlreadyExists = registeredResources
                .Where(m => m.SynopsisResource.Equals(dto.SynopsisResource)
                    || (!string.IsNullOrWhiteSpace(m.TitleResource)
                        && m.TitleResource.Equals(dto.SynopsisResource))
                ).Any();

            if (checkResourceAlreadyExists)
            {
                errors.Add(new ErrorContent("Serie: SynopsisResource", _localizer["AlreadyExists"].Value));
                throw new AppException(_localizer["NotCreated"].Value, errors);
            }

            var languageIso = RegexHelper.RemoveSpecialCharacters(dto.OriginalLanguageIso);

            var newSerie = new Serie()
            {
                Abbreviation = dto.Abbreviation,
                ImdbId = dto.ImdbId,
                OriginalLanguageId = (short)Enum.Parse<LanguageEnum>(languageIso, true).GetHashCode(),
                OriginalName = dto.OriginalName,
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
                    var checkSynopsisAlreadyExists = registeredResources
                        .Where(m => m.SynopsisResource.Equals(dto.SynopsisResource)
                            || (!string.IsNullOrWhiteSpace(m.TitleResource)
                                && m.TitleResource.Equals(dto.SynopsisResource))
                        ).Any();

                    var checkTitleAlreadyExists = registeredResources
                        .Where(m => m.SynopsisResource.Equals(episode.TitleResource)
                            || (!string.IsNullOrWhiteSpace(m.TitleResource) && m.TitleResource.Equals(episode.TitleResource))
                        ).Any();

                    if (checkSynopsisAlreadyExists)
                        errors.Add(new ErrorContent($"Episode {newSeason.Number} x {episode.Number}: SynopsisResource", _localizer["AlreadyExists"].Value));

                    if (checkTitleAlreadyExists)
                        errors.Add(new ErrorContent($"Episode {newSeason.Number} x {episode.Number}: TitleResource", _localizer["AlreadyExists"].Value));

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
            if (errors.Any())
                throw new AppException(_localizer["NotCreated"].Value, errors);

            await _context.Serie.AddAsync(newSerie);
            await _context.SaveChangesAsync();

            var serieRetorned = await _context.Serie.AsNoTracking()
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
                    Timeline = s.TimelineId
                })
                .LastAsync();

            return serieRetorned;
        }
    }
}
