﻿using System.Net;
using Application.Configurations;
using Application.Data.ViewModel;
using AutoMapper;
using CrossCutting.Extensions;
using CrossCutting.Resources;
using Domain;
using Domain.Model;
using Domain.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Application.Services
{
    public class SeasonService
    {
        private readonly StarTrekContext _context;
        private readonly IStringLocalizer<Messages> _localizerMessages;
        private readonly IMapper _mapper;

        public SeasonService(StarTrekContext context, IStringLocalizer<Messages> localizer, IMapper mapper)
        {
            _context = context;
            _localizerMessages = localizer;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SeasonVM>> GetSeasons(byte page, byte pageSize)
        {
            pageSize = pageSize == 0 ? (byte)100 : pageSize;

            return await _context.Season.AsNoTracking()
                .OrderBy(x => x.SeasonId)
                .Skip(page * pageSize)
                .Take(pageSize)
                .Select(x => new SeasonVM(x.SeasonId, x.Number, x.Episodes.ToArray()))
                .ToArrayAsync()
                ?? [];
        }

        public async Task<SeasonVM> GetSeasonById(int seasonId)
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
                .Select(x => new SeasonVM(x.SeasonId, x.Number, x.Episodes.ToArray()))
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

        public async Task<SeasonVM> CreateSeason(CreateSeasonWithSerieIdDto dto)
        {
            var validator = new CreateSeasonWithSerieIdValidation(_mapper, _localizerMessages, _context);
            var validation = await validator.ValidateAsync(dto);
            if (!validation.IsValid)
                throw new AppException(_localizerMessages["OneOrMoreValidationErrorsOccurred"], validation.Errors);

            if (dto.SerieId <= 0)
            {
                var errors = new Dictionary<string, IEnumerable<string>>()
                {
                    { "ID", [_localizerMessages["Invalid"]] }
                };
                throw new AppException(_localizerMessages["InvalidId"], errors, HttpStatusCode.NotFound);
            }

            var serie = await _context.Serie.FirstOrDefaultAsync(x => x.SerieId.Equals(dto.SerieId));

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
                SerieId = dto.SerieId,
                Number = dto.Number,
                Episodes = []
            };

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
            serie.Seasons.Add(newSeason);

            await _context.SaveChangesAsync();
            var sesason = await _context.Season.AsNoTracking().OrderBy(x => x.SeasonId).LastAsync();

            return _mapper.Map<SeasonVM>(sesason);
        }

        public async Task UpdateSeason(byte seasonId, UpdateSeasonDto dto)
        {
            if (seasonId <= 0)
            {
                var errors = new Dictionary<string, IEnumerable<string>>()
                {
                    { "ID", [_localizerMessages["Invalid"]] }
                };
                throw new AppException(_localizerMessages["InvalidId"], errors, HttpStatusCode.NotFound);
            }

            var season = await _context.Season.Where(x => x.SeasonId.Equals(seasonId)).FirstOrDefaultAsync();

            if (season == null)
            {
                var errors = new Dictionary<string, IEnumerable<string>>()
                {
                    { "Season", [_localizerMessages["NotFound"]] }
                };
                throw new AppException(_localizerMessages["NotFound"], errors, HttpStatusCode.NotFound);
            }

            season.SerieId = dto.SerieId;
            season.Number = dto.Number;

            await _context.SaveChangesAsync();

            return;
        }
    }
}
