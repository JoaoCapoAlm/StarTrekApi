using Application.Data.ViewModel;
using AutoMapper;
using CrossCutting.Exceptions;
using CrossCutting.Resources;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Application.Services
{
    public class EpisodeService
    {
        private readonly StarTrekContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<Messages> _localizerMessages;

        public EpisodeService(StarTrekContext context, IMapper mapper, IStringLocalizer<Messages> localizerMessages)
        {
            _context = context;
            _mapper = mapper;
            _localizerMessages = localizerMessages;
        }

        public async Task<IEnumerable<EpisodeWithSeasonIdVM>> GetEpisodeList(byte page, byte pageSize)
        {
            pageSize = pageSize <= 0 ? (byte)100 : pageSize;

            return await _context.Episode.AsNoTracking()
                .Skip(page * pageSize)
                .Take(pageSize)
                .Select(x => _mapper.Map<EpisodeWithSeasonIdVM>(x))
                .ToArrayAsync()
                ?? [];
        }

        public async Task<EpisodeWithSeasonIdVM> GetEpisodeById(int episodeId)
        {
            if (episodeId <= 0)
            {
                var error = new Dictionary<string, IEnumerable<string>>
                {
                    { "ID", [_localizerMessages["InvalidId"].Value] }
                };
                throw new AppException(_localizerMessages["InvalidId"].Value, error);
            }

            var episode = await _context.Episode.AsNoTracking()
                .Where(x => x.EpisodeId.Equals(episodeId))
                .Select(x => _mapper.Map<EpisodeWithSeasonIdVM>(x))
                .FirstOrDefaultAsync();

            if (episode == null)
            {
                var error = new Dictionary<string, IEnumerable<string>>
                {
                    { "ID", [_localizerMessages["NotFound"].Value] }
                };
                throw new AppException(_localizerMessages["NotFound"].Value, error, System.Net.HttpStatusCode.NotFound);
            }

            return episode;
        }
    }
}
