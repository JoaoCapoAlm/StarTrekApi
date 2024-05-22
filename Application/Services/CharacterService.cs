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
    public class CharacterService : ICharacterService
    {
        private readonly StarTrekContext _context;
        private readonly IStringLocalizer<Messages> _localizer;
        private readonly IStringLocalizer<PlacesResource> _placesLocalizer;
        private readonly IMapper _mapper;

        public CharacterService(StarTrekContext context,
            IMapper mapper,
            IStringLocalizer<Messages> localizer,
            IStringLocalizer<PlacesResource> placesLocalizer)
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
            _placesLocalizer = placesLocalizer;
        }

        public async Task<CharacterVM> GetById(int id)
        {
            if (id <= 0)
            {
                var error = new Dictionary<string, IEnumerable<string>>
                {
                    { "ID", [_localizer["InvalidId"].Value] }
                };
                throw new AppException(_localizer["InvalidId"].Value, error);
            }

            var character = await _context.Character
                .AsNoTracking()
                .Where(x => x.CharacterId.Equals(id))
                .Include(x => x.Species)
                    .ThenInclude(x => x.Planet)
                    .ThenInclude(x => x.PlaceType)
                .Include(x => x.Species)
                    .ThenInclude(x => x.Planet)
                    .ThenInclude(x => x.Quadrant)
                .Select(x => _mapper.Map<CharacterVM>(x))
                .FirstOrDefaultAsync();

            if (character is null)
            {
                var error = new Dictionary<string, IEnumerable<string>>
                {
                    { "ID", [_localizer["NotFound"].Value] }
                };
                throw new AppException(_localizer["NotFound"].Value, error, System.Net.HttpStatusCode.NotFound);
            }

            character.Species.Name = _localizer[character.Species.Name];
            character.Species.Planet.Name = _placesLocalizer[character.Species.Planet.Name];
            character.Species.Planet.Quadrant.Name = _placesLocalizer[character.Species.Planet.Quadrant.Name];
            character.Species.Planet.PlaceType.Type = _placesLocalizer[character.Species.Planet.PlaceType.Type];

            return character;
        }

        public async Task<IEnumerable<CharacterVM>> GetList(byte page, byte pageSize)
        {
            var list = await _context.Character
                .AsNoTracking()
                .OrderBy(x => x.CharacterId)
                .Skip(page * pageSize)
                .Take(pageSize)
                .Include(x => x.Species)
                    .ThenInclude(x => x.Planet)
                    .ThenInclude(x => x.PlaceType)
                .Include(x => x.Species)
                    .ThenInclude(x => x.Planet)
                    .ThenInclude(x => x.Quadrant)
                .Select(x => _mapper.Map<CharacterVM>(x))
            .ToListAsync();

            Parallel.ForEach(list, x =>
            {
                x.Species.Name = _localizer[x.Species.Name];
                x.Species.Planet.Name = _placesLocalizer[x.Species.Planet.Name];
                x.Species.Planet.Quadrant.Name = _placesLocalizer[x.Species.Planet.Quadrant.Name];
                x.Species.Planet.PlaceType.Type = _placesLocalizer[x.Species.Planet.PlaceType.Type];
            });

            return list;
        }

        public async Task<CharacterVM> Create(CreateCharacterDto dto)
        {
            var validator = new CreateCharacterValidation(_localizer, _context);
            await validator.ValidateAndThrowAsyncStarTrek(dto, _localizer["OneOrMoreValidationErrorsOccurred"]);

            var character = _mapper.Map<Character>(dto);
            var operation = await _context.Character.AddAsync(character);
            await _context.SaveChangesAsync();

            return _mapper.Map<CharacterVM>(operation.Entity);
        }
    }
}
