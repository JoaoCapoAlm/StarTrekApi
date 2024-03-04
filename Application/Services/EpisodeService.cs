using Application.Data.ViewModel;
using AutoMapper;
using CrossCutting.Exceptions;
using CrossCutting.Extensions;
using CrossCutting.Resources;
using Domain;
using Domain.Model;
using Domain.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Application.Services
{
    public class EpisodeService
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

        public async Task<IEnumerable<EpisodeWithSeasonIdVM>> GetEpisodeList(byte page, byte pageSize)
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
                ep.TitleTranslated = _localizerTitleSynopsis[ep.TitleTranslated];
                ep.SynopsisTranslated = _localizerTitleSynopsis[ep.SynopsisTranslated];
            }

            return epArray;
        }

        public async Task<EpisodeWithSeasonIdVM> GetEpisodeById(int episodeId)
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

            episode.SynopsisTranslated = _localizerTitleSynopsis[episode.SynopsisTranslated];
            episode.TitleTranslated = _localizerTitleSynopsis[episode.TitleTranslated];

            return episode;
        }

        public async Task<EpisodeWithSeasonIdVM> CreateEpisode(CreateEpisodeWithSeasonIdDto dto)
        {
            var validator = new CreateEpisodeWithSeasonIdValidation(_mapper, _localizer, _context);
            await validator.ValidateAndThrowAsyncStarTrek(dto, _localizer["OneOrMoreValidationErrorsOccurred"]);

            Episode ep = _mapper.Map<Episode>(dto);
            ep.SynopsisResource = dto.TitleResource.CreateSynopsisResource();

            await _context.Episode.AddAsync(ep);
            await _context.SaveChangesAsync();

            var epMap = _mapper.Map<EpisodeWithSeasonIdVM>(ep);

            epMap.TitleTranslated = _localizerTitleSynopsis[epMap.TitleTranslated];
            epMap.SynopsisTranslated = _localizerTitleSynopsis[epMap.SynopsisTranslated];

            return epMap;
        }
    }
}
