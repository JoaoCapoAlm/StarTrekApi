using System.Linq.Expressions;
using AutoMapper;
using CrossCutting.Exceptions;
using CrossCutting.Resources;
using Domain;
using Domain.Interfaces;
using Domain.Model;
using Domain.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Application.Services
{
    public class CrewService : ICrewService
    {
        private readonly StarTrekContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<Messages> _localizer;
        private readonly IStringLocalizer<PlacesResource> _placesResource;

        public CrewService(StarTrekContext context,
            IMapper mapper,
            IStringLocalizer<Messages> localizer,
            IStringLocalizer<PlacesResource> placesResource
        )
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
            _placesResource = placesResource;
        }

        public async Task<IEnumerable<CrewVM>> GetList(byte page, byte pageSize, Expression<Func<Crew, bool>> predicate)
        {
            pageSize = pageSize > 100 ? (byte)100 : pageSize;

            var list = await _context.Crew
                .AsNoTracking()
                .Where(predicate)
                .OrderBy(c => c.CrewId)
                .Skip(page * pageSize)
                .Take(pageSize)
                .Include(x => x.Country)
                .Select(x => _mapper.Map<CrewVM>(x))
                .ToArrayAsync()
                ?? [];

            Parallel.ForEach(list, crew =>
            {
                crew.Country = _placesResource[crew.Country];
            });

            return list;
        }

        public async Task<CrewVM> GetById(int crewId)
        {
            if (crewId <= 0)
            {
                var error = new Dictionary<string, IEnumerable<string>>
                {
                    { "ID", [_localizer["InvalidId"].Value] }
                };
                throw new AppException(_localizer["InvalidId"].Value, error);
            }

            var crew = await _context.Crew
                .AsNoTracking()
                .Include(x => x.Country)
                .Where(c => c.CrewId.Equals(crewId))
                .Select(x => _mapper.Map<CrewVM>(x))
                .FirstOrDefaultAsync();

            if (crew == null)
            {
                var error = new Dictionary<string, IEnumerable<string>>()
                {
                    { "id", [_localizer["NotFound"]] }
                };
                throw new AppException(_localizer["NotFound"].Value, error, System.Net.HttpStatusCode.NotFound);
            }

            crew.Country = _placesResource[crew.Country];

            return crew;
        }
    }
}
