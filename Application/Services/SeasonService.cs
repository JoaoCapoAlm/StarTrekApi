using System.Linq.Expressions;
using System.Net;
using AutoMapper;
using CrossCutting.Exceptions;
using CrossCutting.Extensions;
using CrossCutting.Resources;
using Domain;
using Domain.Interfaces;
using Domain.Model;
using Domain.Validation;
using Domain.ViewModel;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Application.Services
{
    public class SeasonService : ISeasonService
    {
        private readonly StarTrekContext _context;
        private readonly IStringLocalizer<Messages> _localizerMessages;
        private readonly IMapper _mapper;

        public SeasonService(StarTrekContext context,
            IStringLocalizer<Messages> localizer,
            IMapper mapper
        )
        {
            _context = context;
            _localizerMessages = localizer;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SeasonWithSerieIdVM>> GetList(byte page, byte pageSize, Expression<Func<Season, bool>> predicate)
        {
            pageSize = pageSize == 0 ? (byte)100 : pageSize;

            return await _context.Season.AsNoTracking()
                .Include(x => x.Episodes)
                .OrderBy(x => x.SeasonId)
                .Skip(page * pageSize)
                .Take(pageSize)
                .Select(x => _mapper.Map<SeasonWithSerieIdVM>(x))
                .ToArrayAsync()
                ?? [];
        }
        public async Task<SeasonWithSerieIdVM> GetById(short seasonId)
        {
            if (seasonId <= 0)
            {
                var error = new Dictionary<string, IEnumerable<string>>
                {
                    { "ID", [_localizerMessages["InvalidId"].Value] }
                };
                throw new AppException(_localizerMessages["InvalidId"].Value, error);
            }

            var season = await _context.Season.AsNoTracking()
                .Where(x => x.SeasonId == seasonId)
                .Select(x => new SeasonWithSerieIdVM(x.SeasonId, x.SerieId, x.Number, x.Episodes.ToArray()))
                .FirstOrDefaultAsync();

            if (season == null)
            {
                var errors = new Dictionary<string, IEnumerable<string>>()
                {
                    { "id", [_localizerMessages["NotFound"]] }
                };
                throw new AppException(_localizerMessages["NotFound"], errors, HttpStatusCode.NotFound);
            }

            return season;
        }
        public async Task<SeasonWithSerieIdVM> Create(CreateSeasonWithSerieIdDto dto)
        {
            var validator = new CreateSeasonWithSerieIdValidation(_mapper, _localizerMessages, _context);
            await validator.ValidateAndThrowAsyncStarTrek(dto, _localizerMessages["OneOrMoreValidationErrorsOccurred"]);

            var serie = await _context.Serie.Where(x => x.SerieId.Equals(dto.SerieId)).FirstOrDefaultAsync();

            if (serie == null)
            {
                var errors = new Dictionary<string, IEnumerable<string>>()
                {
                    { "Serie", [_localizerMessages["NotFound"]] }
                };
                throw new AppException(_localizerMessages["NotFound"], errors, HttpStatusCode.NotFound);
            }

            var newSeason = new Season()
            {
                Number = dto.Number,
                Episodes = []
            };

            if (dto.Episodes != null)
            {
                Parallel.ForEach(dto.Episodes, new ParallelOptions(), episode =>
                {
                    newSeason.Episodes.Add(new Episode
                    {
                        ImdbId = episode.ImdbId,
                        Number = episode.Number,
                        RealeaseDate = episode.RealeaseDate,
                        StardateFrom = episode.StardateFrom,
                        StardateTo = episode.StardateTo,
                        SynopsisResource = episode.TitleResource.CreateSynopsisResource(),
                        Time = episode.Time,
                        TitleResource = episode.TitleResource
                    });
                });
            }
            serie.Seasons ??= [];
            serie.Seasons.Add(newSeason);

            await _context.SaveChangesAsync();

            return _mapper.Map<SeasonWithSerieIdVM>(newSeason);
        }
        public async Task Update(short seasonId, UpdateSeasonDto dto)
        {
            var validator = new UpdateSeasonValidation(_localizerMessages);
            validator.ValidateAndThrowStarTrek(dto, _localizerMessages["OneOrMoreValidationErrorsOccurred"]);

            if (seasonId <= 0)
            {
                var errors = new Dictionary<string, IEnumerable<string>>()
                {
                    { "ID", [_localizerMessages["Invalid"]] }
                };
                throw new AppException(_localizerMessages["InvalidId"], errors, HttpStatusCode.NotFound);
            }

            var season = await _context.Season
                .Where(x => x.SeasonId.Equals(seasonId))
                .FirstOrDefaultAsync();

            if (season == null)
            {
                var errors = new Dictionary<string, IEnumerable<string>>()
                {
                    { "Season", [_localizerMessages["NotFound"]] }
                };
                throw new AppException(_localizerMessages["NotFound"], errors, HttpStatusCode.NotFound);
            }

            season.SerieId = dto.SerieId ?? season.SerieId;
            season.Number = dto.Number ?? season.Number;

            await _context.SaveChangesAsync();
        }
    }
}
