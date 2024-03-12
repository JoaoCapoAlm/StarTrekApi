using System.Linq.Expressions;
using AutoMapper;
using CrossCutting.Exceptions;
using CrossCutting.Resources;
using Domain;
using Domain.DTOs;
using Domain.Interfaces;
using Domain.Model;
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

            return species;
        }

        public async Task<IEnumerable<SpeciesVM>> GetList(byte page, byte pageSize, Expression<Func<Species, bool>> predicate)
        {
            pageSize = pageSize >= 100 ? (byte)100 : pageSize;

            var list = await _context.Species.AsNoTracking()
                .Include(x => x.Planet)
                .ThenInclude(x => x.Quadrant)
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
            });

            return list;
        }

        public async Task<SpeciesVM> CreateSpecies(CreateSpeciesDto dto)
        {
            var species = _mapper.Map<Species>(dto);

            await _context.Species.AddAsync(species);
            await _context.SaveChangesAsync();

            var speciesVM = _mapper.Map<SpeciesVM>(species);

            speciesVM.Name = _localizer[speciesVM.Name];
            speciesVM.Planet.Name = _placesLocalizer[speciesVM.Planet.Name];
            speciesVM.Planet.Quadrant.Name = _placesLocalizer[speciesVM.Planet.Quadrant.Name];

            return speciesVM;
        }
    }
}
