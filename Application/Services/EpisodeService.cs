using AutoMapper;
using CrossCutting.Exceptions;
using CrossCutting.Extensions;
using CrossCutting.Resources;
using Domain;
using Domain.Interfaces;
using Domain.Model;
using Domain.Validation;
using Domain.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Application.Services
{
    public class EpisodeService : IEpisodeService
    {
        private readonly StarTrekContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<Messages> _localizer;
        private readonly IStringLocalizer<TitleSynopsis> _localizerTitleSynopsis;

        public EpisodeService(StarTrekContext context,
            IMapper mapper,
            IStringLocalizer<Messages> localizer,
            IStringLocalizer<TitleSynopsis> localizerTitleSynopsis)
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
            _localizerTitleSynopsis = localizerTitleSynopsis;
        }

        public async Task<IEnumerable<EpisodeWithSeasonIdVM>> GetList(byte page, byte pageSize)
        {
            pageSize = pageSize <= 0 ? (byte)100 : pageSize;

            var epArray = await _context.Episode.AsNoTracking()
                .Skip(page * pageSize)
                .Take(pageSize)
                .Select(x => _mapper.Map<EpisodeWithSeasonIdVM>(x))
                .ToArrayAsync();

            if (epArray == null)
                return [];

            foreach (var ep in epArray)
            {
                ep.TranslatedTitle = _localizerTitleSynopsis[ep.TranslatedTitle];
                ep.TranslatedSynopsis = _localizerTitleSynopsis[ep.TranslatedSynopsis];
            }

            return epArray;
        }

        public async Task<EpisodeWithSeasonIdVM> GetById(int episodeId)
        {
            if (episodeId <= 0)
            {
                var error = new Dictionary<string, IEnumerable<string>>
                {
                    { "ID", [_localizer["InvalidId"].Value] }
                };
                throw new AppException(_localizer["InvalidId"].Value, error);
            }

            var episode = await _context.Episode.AsNoTracking()
                .Where(x => x.EpisodeId.Equals(episodeId))
                .Select(x => _mapper.Map<EpisodeWithSeasonIdVM>(x))
                .FirstOrDefaultAsync();

            if (episode == null)
            {
                var error = new Dictionary<string, IEnumerable<string>>
                {
                    { "ID", [_localizer["NotFound"].Value] }
                };
                throw new AppException(_localizer["NotFound"].Value, error, System.Net.HttpStatusCode.NotFound);
            }

            episode.TranslatedSynopsis = _localizerTitleSynopsis[episode.TranslatedSynopsis];
            episode.TranslatedTitle = _localizerTitleSynopsis[episode.TranslatedTitle];

            return episode;
        }

        public async Task<EpisodeWithSeasonIdVM> Create(CreateEpisodeWithSeasonIdDto dto)
        {
            var validator = new CreateEpisodeWithSeasonIdValidation(_mapper, _localizer, _context);
            await validator.ValidateAndThrowAsyncStarTrek(dto, _localizer["OneOrMoreValidationErrorsOccurred"]);

            Episode ep = _mapper.Map<Episode>(dto);
            ep.SynopsisResource = dto.TitleResource.CreateSynopsisResource();

            await _context.Episode.AddAsync(ep);
            await _context.SaveChangesAsync();

            return _mapper.Map<EpisodeWithSeasonIdVM>(ep);
        }

        public async Task Update(int episodeId, UpdateEpisodeDto dto)
        {
            var validator = new UpdateEpisodeValidation(_localizer, _context);
            validator.ValidateAndThrowStarTrek<UpdateEpisodeDto>(dto, _localizer["OneOrMoreValidationErrorsOccurred"]);

            if (episodeId <= 0)
            {
                var error = new Dictionary<string, IEnumerable<string>>
                {
                    { "ID", [_localizer["InvalidId"].Value] }
                };
                throw new AppException(_localizer["InvalidId"].Value, error);
            }

            var episode = await _context.Episode
                .Where(x => x.EpisodeId.Equals(episodeId))
                .FirstOrDefaultAsync();

            if (episode == null)
            {
                var error = new Dictionary<string, IEnumerable<string>>
                {
                    { "ID", [_localizer["NotFound"].Value] }
                };
                throw new AppException(_localizer["NotFound"].Value, error, System.Net.HttpStatusCode.NotFound);
            }

            episode.Number = dto.Number ?? episode.Number;
            episode.ImdbId = string.IsNullOrWhiteSpace(dto.ImdbId) ? episode.ImdbId : dto.ImdbId;
            episode.RealeaseDate = dto.RealeaseDate ?? episode.RealeaseDate;
            episode.SeasonId = dto.SeasonId ?? episode.SeasonId;
            episode.StardateFrom = dto.StardateFrom ?? episode.StardateFrom;
            episode.StardateTo = dto.StardateTo ?? episode.StardateFrom;
            episode.Time = dto.Time ?? episode.Time;

            await _context.SaveChangesAsync();
        }
    }
}
