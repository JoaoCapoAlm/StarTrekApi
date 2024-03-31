using AutoMapper;
using CrossCutting.Exceptions;
using CrossCutting.Extensions;
using CrossCutting.Resources;
using Domain;
using Domain.DTOs;
using Domain.Interfaces;
using Domain.Model;
using Domain.Validation;
using Domain.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Application.Services
{
    public class SpeciesService : ISpeciesService
    {
        private readonly IMapper _mapper;
        private readonly StarTrekContext _context;
        private readonly IStringLocalizer<Messages> _localizer;
        private readonly IStringLocalizer<PlacesResource> _placesLocalizer;
        public SpeciesService(IMapper mapper,
            StarTrekContext context,
            IStringLocalizer<Messages> localizer,
            IStringLocalizer<PlacesResource> placesLocalizer)
        {
            _mapper = mapper;
            _context = context;
            _localizer = localizer;
            _placesLocalizer = placesLocalizer;
        }

        public async Task<SpeciesVM> GetById(short speciesId)
        {
            if (speciesId <= 0)
            {
                var error = new Dictionary<string, IEnumerable<string>>
                {
                    { "ID", [_localizer["InvalidId"].Value] }
                };
                throw new AppException(_localizer["InvalidId"].Value, error);
            }

            var species = await _context.Species.AsNoTracking()
                .Include(x => x.Planet).ThenInclude(x => x.Quadrant)
                .Include(x => x.Planet).ThenInclude(x => x.PlaceType)
                .Where(x => x.SpeciesId.Equals(speciesId))
                .Select(x => _mapper.Map<SpeciesVM>(x))
                .FirstOrDefaultAsync();

            if (species is null)
            {
                var error = new Dictionary<string, IEnumerable<string>>
                {
                    { "ID", [_localizer["NotFound"].Value] }
                };
                throw new AppException(_localizer["NotFound"].Value, error, System.Net.HttpStatusCode.NotFound);
            }

            species.Name = _localizer[species.Name];
            species.Planet.Name = _placesLocalizer[species.Planet.Name];
            species.Planet.Quadrant.Name = _placesLocalizer[species.Planet.Quadrant.Name];
            species.Planet.PlaceType.Type = _placesLocalizer[species.Planet.PlaceType.Type];

            return species;
        }

        public async Task<IEnumerable<SpeciesVM>> GetList(byte page, byte pageSize)
        {
            pageSize = pageSize >= 100 ? (byte)100 : pageSize;

            var list = await _context.Species.AsNoTracking()
                .Include(x => x.Planet).ThenInclude(x => x.Quadrant)
                .Include(x => x.Planet).ThenInclude(x => x.PlaceType)
                .Skip(page * pageSize)
                .Take(pageSize)
                .Select(x => _mapper.Map<SpeciesVM>(x))
                .ToArrayAsync()
                ?? [];

            Parallel.ForEach(list, species =>
            {
                species.Name = _localizer[species.Name];
                species.Planet.Name = _placesLocalizer[species.Planet.Name];
                species.Planet.Quadrant.Name = _placesLocalizer[species.Planet.Quadrant.Name];
                species.Planet.PlaceType.Type = _placesLocalizer[species.Planet.PlaceType.Type];
            });

            return list;
        }

        public async Task<SpeciesVM> CreateSpecies(CreateSpeciesDto dto)
        {
            var validator = new CreateSpeciesValidation(_context, _localizer);
            await validator.ValidateAndThrowAsyncStarTrek(dto, _localizer["OneOrMoreValidationErrorsOccurred"]);

            var species = _mapper.Map<Species>(dto);

            await _context.Species.AddAsync(species);
            await _context.SaveChangesAsync();

            var speciesVM = _mapper.Map<SpeciesVM>(species);

            speciesVM.Planet = await _context.Place.AsNoTracking()
                .Include(x => x.Quadrant)
                .Include(x => x.PlaceType)
                .Where(x => x.PlaceId.Equals(species.PlanetId))
                .Select(x => _mapper.Map<PlaceVM>(x))
                .FirstAsync();

            speciesVM.Name = _localizer[speciesVM.Name];
            speciesVM.Planet.Name = _placesLocalizer[speciesVM.Planet.Name];
            speciesVM.Planet.Quadrant.Name = _placesLocalizer[speciesVM.Planet.Quadrant.Name];
            speciesVM.Planet.PlaceType.Type = _placesLocalizer[speciesVM.Planet.PlaceType.Type];

            return speciesVM;
        }
    }
}
