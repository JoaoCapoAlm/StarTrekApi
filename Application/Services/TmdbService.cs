using CrossCutting.Extensions;
using CrossCutting.Resources;
using Domain;
using Domain.DTOs;
using Domain.Model;
using Domain.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using TMDB;

namespace Application.Services
{
    public class TmdbService
    {
        private readonly StarTrekContext _context;
        private readonly IStringLocalizer<Messages> _localizer;
        public TmdbService(StarTrekContext context, IStringLocalizer<Messages> localizer)
        {
            _context = context;
            _localizer = localizer;
        }
        public async Task<SerieVM> CreateNewSerieByTmdb(int tmdbId, CreateNewSerieByTmdbDto dto)
        {
            Serie serie = await _context.Serie?.Where(s => s.TmdbId == tmdbId)?
                .Include(x => x.Timeline)
                .Include(s => s.Seasons).ThenInclude(s => s.Episodes)
                .Include(s => s.Language)
                .FirstOrDefaultAsync();
            if (serie is not null && serie.DateSyncTmdb.HasValue)
                return new SerieVM
                {
                    ID = serie.SerieId,
                    OriginalName = serie.OriginalName,
                    Abbreviation = serie.Abbreviation,
                    ImdbId = serie.ImdbId,
                    Timeline = new TimelineVM(serie.TimelineId, serie.Timeline.Name),
                    Seasons = serie.Seasons.Select(s => new SeasonVM(s.SeasonId, s.Number, s.Episodes)).ToArray(),
                    OriginalLanguage = new LanguageVM()
                    {
                        CodeISO = serie.Language.CodeISO,
                        Name = serie.Language.ResourceName
                    }
                };

            var searchSerie = await new TmdbAPI().SearchSerie(tmdbId) ?? throw new Exception(_localizer["notFound"].Value);

            bool isNew = serie is null;
            serie ??= new Serie();

            serie.DateSyncTmdb = DateTime.UtcNow;
            serie.OriginalName = searchSerie.original_name;
            serie.SynopsisResource = string.IsNullOrWhiteSpace(dto.SynopsisResource) ? searchSerie.original_name.CreateResourceName("Synopsis") : dto.SynopsisResource;
            serie.Abbreviation = string.IsNullOrWhiteSpace(dto.Abbreviation) ? serie.Abbreviation : dto.Abbreviation;
            serie.TmdbId = tmdbId;
            serie.ImdbId = string.IsNullOrWhiteSpace(dto.Imdb) ? serie.ImdbId : dto.Imdb;
            serie.TimelineId = dto.Timeline;

            short? originalLanguageId = await _context.Language
                                            .AsNoTracking()
                                            .Where(l => l.CodeISO.Equals(searchSerie.original_language))
                                            .Select(l => l.LanguageId)
                                            .FirstOrDefaultAsync();
            serie.OriginalLanguageId = originalLanguageId ?? 1;

            if (isNew)
            {
                await _context.Serie.AddAsync(serie);
                await _context.SaveChangesAsync();
                serie = await _context.Serie?.Where(s => s.TmdbId == tmdbId)?
                .Include(s => s.Seasons).ThenInclude(s => s.Episodes)
                .Include(s => s.Language)
                .FirstOrDefaultAsync();
            }

            var seassons = new List<Season>();
            Parallel.ForEach(searchSerie.seasons.Where(s => s.season_number != 0).ToArray(), (s) =>
            {
                seassons.Add(new Season
                {
                    Number = (byte)s.season_number,
                    SerieId = serie.SerieId
                });
            });

            await _context.Season.AddRangeAsync(seassons);
            await _context.SaveChangesAsync();

            serie = await _context.Serie.Where(s => s.SerieId.Equals(serie.SerieId))
                .Include(s => s.Seasons).ThenInclude(s => s.Episodes)
                .Include(s => s.Language)
                .FirstOrDefaultAsync();

            var episodes = await _context.Episode.AsNoTracking()
                .Where(e => seassons.Select(s => s.SeasonId)
                                    .ToArray()
                                    .Contains(e.SeasonId))
                .ToArrayAsync()
                ?? [];

            return new SerieVM
            {
                ID = serie.SerieId,
                OriginalName = serie.OriginalName,
                Abbreviation = serie.Abbreviation,
                ImdbId = serie.ImdbId,
                Timeline = new TimelineVM(serie.TimelineId, serie.Timeline.Name),
                Seasons = serie.Seasons.Select(s => new SeasonVM(s.SeasonId, s.Number, episodes)).ToArray(),
                OriginalLanguage = new LanguageVM()
                {
                    CodeISO = serie.Language.CodeISO,
                    Name = serie.Language.ResourceName
                }
            };
        }
    }
}
