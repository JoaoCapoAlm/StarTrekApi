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
    public class PlaceService : IPlaceService
    {
        private readonly StarTrekContext _context;
        private readonly IStringLocalizer<Messages> _localizer;
        private readonly IStringLocalizer<PlacesResource> _placesLocalizer;
        private readonly IMapper _mapper;
        public PlaceService(StarTrekContext context,
            IStringLocalizer<Messages> localizer,
            IStringLocalizer<PlacesResource> placesLocalizer,
            IMapper mapper)
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
            _placesLocalizer = placesLocalizer;
        }

        public async Task<PlaceVM> GetById(short placeId)
        {
            if (placeId <= 0)
            {
                var error = new Dictionary<string, IEnumerable<string>>
                {
                    { "ID", [_localizer["InvalidId"].Value] }
                };
                throw new AppException(_localizer["InvalidId"].Value, error);
            }

            var place = await _context.Place.AsNoTracking()
                .Where(x => x.PlaceId.Equals(placeId))
                .Include(x => x.Quadrant)
                .Include(x => x.PlaceType)
                .Select(x => _mapper.Map<PlaceVM>(x))
                .FirstOrDefaultAsync();

            if (place is null)
            {
                var error = new Dictionary<string, IEnumerable<string>>
                {
                    { "ID", [_localizer["NotFound"].Value] }
                };
                throw new AppException(_localizer["NotFound"].Value, error, System.Net.HttpStatusCode.NotFound);
            }

            place.Name = _placesLocalizer[place.Name];
            place.PlaceType.Type = _placesLocalizer[place.PlaceType.Type];
            place.Quadrant.Name = _placesLocalizer[place.Quadrant.Name];

            return place;
        }

        public async Task<IEnumerable<PlaceVM>> GetList(
            byte page,
            byte pageSize,
            Expression<Func<Place, bool>> predicate
        )
        {
            var list = await _context.Place.AsNoTracking()
                .Where(predicate)
                .OrderBy(x => x.PlaceId)
                .Skip(page * pageSize)
                .Take(pageSize)
                .Include(x => x.Quadrant)
                .Include(x => x.PlaceType)
                .Select(x => _mapper.Map<PlaceVM>(x))
                .ToArrayAsync()
                ?? [];

            Parallel.ForEach(list, x =>
            {
                x.Name = _placesLocalizer[x.Name];
                x.PlaceType.Type = _placesLocalizer[x.PlaceType.Type];
                x.Quadrant.Name = _placesLocalizer[x.Quadrant.Name];
            });

            return list;
        }
    }
}
