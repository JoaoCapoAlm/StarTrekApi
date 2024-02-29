using System.Net;
using Application.Configurations;
using Application.Data.ViewModel;
using CrossCutting.Resources;
using Domain;
using Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Application.Services
{
    public class SeasonService
    {
        private readonly StarTrekContext _context;
        private readonly IStringLocalizer<Messages> _localizer;
        private readonly IStringLocalizer<TitleSynopsis> _titleSynopsisLocalizer;

        public SeasonService(StarTrekContext context, IStringLocalizer<Messages> localizer,
            IStringLocalizer<TitleSynopsis> titleSynopsisLocalizer)
        {
            _context = context;
            _localizer = localizer;
            _titleSynopsisLocalizer = titleSynopsisLocalizer;
        }

        public async Task<IEnumerable<SeasonVM>> GetSeasons(byte page, byte pageSize)
        {
            pageSize = pageSize == 0 ? (byte)100 : pageSize;

            return await _context.Season.AsNoTracking()
                .OrderBy(x => x.SeasonId)
                .Skip(page * pageSize)
                .Take(pageSize)
                .Select(x => new SeasonVM(x.SeasonId, x.Number, x.Episodes.ToArray()))
                .ToArrayAsync()
                ?? [];
        }

        public async Task<SeasonVM> GetSeasonById(int seasonId)
        {
            if (seasonId <= 0)
            {
                var error = new Dictionary<string, IEnumerable<string>>
                {
                    { "ID", [_localizer["InvalidId"].Value] }
                };
                throw new AppException(_localizer["InvalidId"].Value, error);
            }

            var season = await _context.Season.AsNoTracking()
                .Where(x => x.SeasonId == seasonId)
                .Select(x => new SeasonVM(x.SeasonId, x.Number, x.Episodes.ToArray()))
                .FirstOrDefaultAsync();

            if (season == null)
            {
                var errors = new Dictionary<string, IEnumerable<string>>()
                {
                    { "id", [_localizer["NotFound"]] }
                };
                throw new AppException(_localizer["NotFound"], errors, HttpStatusCode.NotFound);
            }

            return season;
        }

        public async Task<SeasonVM> CreateSeason(CreateSeasonWithSerieIdDto dto)
        {
            if (dto.SerieId <= 0)
            {
                var errors = new Dictionary<string, IEnumerable<string>>()
                {
                    { "ID", [_localizer["Invalid"]] }
                };
                throw new AppException(_localizer["InvalidId"], errors, HttpStatusCode.NotFound);
            }

            var serie = await _context.Serie.FirstOrDefaultAsync(x => x.SerieId.Equals(dto.SerieId));

            if (serie == null)
            {
                var errors = new Dictionary<string, IEnumerable<string>>()
                {
                    { "Serie", [_localizer["NotFound"]] }
                };
                throw new AppException(_localizer["NotFound"], errors, HttpStatusCode.NotFound);
            }

            var newSeason = new Season()
            {
                SerieId = dto.SerieId,
                Number = dto.Number,
                Episodes = []
            };

            Parallel.ForEach(dto.Episodes, new ParallelOptions(), episode =>
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
            serie.Seasons.Add(newSeason);

            await _context.SaveChangesAsync();

            return new SeasonVM(newSeason.SeasonId, newSeason.Number, newSeason.Episodes);
        }

        public async Task UpdateSeason(byte seasonId, UpdateSeasonDto dto)
        {
            if (seasonId <= 0)
            {
                var errors = new Dictionary<string, IEnumerable<string>>()
                {
                    { "ID", [_localizer["Invalid"]] }
                };
                throw new AppException(_localizer["InvalidId"], errors, HttpStatusCode.NotFound);
            }

            var season = await _context.Season.Where(x => x.SeasonId.Equals(seasonId)).FirstOrDefaultAsync();

            if (season == null)
            {
                var errors = new Dictionary<string, IEnumerable<string>>()
                {
                    { "Season", [_localizer["NotFound"]] }
                };
                throw new AppException(_localizer["NotFound"], errors, HttpStatusCode.NotFound);
            }

            season.SerieId = dto.SerieId;
            season.Number = dto.Number;

            await _context.SaveChangesAsync();

            return;
        }
    }
}
