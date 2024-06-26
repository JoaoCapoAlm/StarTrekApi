﻿using AutoMapper;
using CrossCutting.Enums;
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

        public async Task<IEnumerable<PlaceVM>> GetList(byte page, byte pageSize, QuadrantEnum? quadrant)
        {
            var list = await _context.Place.AsNoTracking()
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

        public async Task<PlaceVM> Create(CreatePlaceDto dto)
        {
            var validator = new CreatePlaceValidation(_context, _localizer);
            await validator.ValidateAndThrowAsyncStarTrek(dto, _localizer["OneOrMoreValidationErrorsOccurred"]);

            var place = _mapper.Map<Place>(dto);

            await _context.Place.AddAsync(place);
            await _context.SaveChangesAsync();

            var vm = _mapper.Map<PlaceVM>(place);

            vm.Quadrant = await _context.Quadrant.AsNoTracking()
                .Where(x => x.QuadrantId.Equals(place.QuadrantId))
                .Select(x => _mapper.Map<QuadrantVM>(x))
                .FirstAsync();

            vm.PlaceType = await _context.PlaceType.AsNoTracking()
                .Where(x => x.PlaceTypeId.Equals(place.PlaceTypeId))
                .Select(x => _mapper.Map<PlaceTypeVM>(x))
                .FirstAsync();

            vm.Quadrant.Name = _placesLocalizer[vm.Quadrant.Name];
            vm.PlaceType.Type = _placesLocalizer[vm.PlaceType.Type];

            return vm;
        }
    }
}
