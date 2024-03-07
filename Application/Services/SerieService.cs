﻿using System.Linq.Expressions;
using System.Net;
using AutoMapper;
using CrossCutting.Enums;
using CrossCutting.Exceptions;
using CrossCutting.Extensions;
using CrossCutting.Helpers;
using CrossCutting.Resources;
using Domain;
using Domain.Interfaces;
using Domain.Model;
using Domain.Validation;
using Domain.ViewModel;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Application.Services
{
    public class SerieService : ISerieService
    {
        private readonly StarTrekContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<Messages> _localizer;
        private readonly IStringLocalizer<TitleSynopsis> _titleSynopsisLocalizer;

        public SerieService(StarTrekContext context,
            IMapper mapper,
            IStringLocalizer<Messages> localizer,
            IStringLocalizer<TitleSynopsis> titleSynopsisLocalizer
        ) {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
            _titleSynopsisLocalizer = titleSynopsisLocalizer;
        }

        public async Task<IEnumerable<SerieVM>> GetList(byte page, byte pageSize, Expression<Func<Serie, bool>> predicate)
        {
            pageSize = pageSize == 0 ? (byte)100 : pageSize;

            var list = await _context.Serie
                .AsNoTracking()
                .Include(x => x.Seasons)
                .ThenInclude(x => x.Episodes)
                .AsSplitQuery()
                .OrderBy(s => s.SerieId)
                .Skip(page * pageSize)
                .Take(pageSize)
                .Select(x => _mapper.Map<SerieVM>(x))
                .ToArrayAsync()
                ?? [];

            Parallel.ForEach(list, serie => {
                Parallel.ForEach(serie.Seasons, season => {
                    Parallel.ForEach(season.Episodes, episode =>
                    {
                        episode.TranslatedSynopsis = _titleSynopsisLocalizer[episode.TranslatedSynopsis];
                        episode.TranslatedTitle = _titleSynopsisLocalizer[episode.TranslatedTitle];
                    });
                });
            });

            return list;
        }

        public async Task<SerieVM> GetById(short id)
        {
            if (id <= 0)
            {
                var error = new Dictionary<string, IEnumerable<string>>
                {
                    { "ID", [_localizer["InvalidId"].Value] }
                };
                throw new AppException(_localizer["InvalidId"].Value, error);
            }

            var serie = await _context.Serie
                .AsNoTracking()
                .Where(s => s.SerieId.Equals(id))
                .Select(s => new SerieVM
                {
                    ID = s.SerieId,
                    OriginalName = s.OriginalName,
                    OriginalLanguage = s.Language.CodeISO,
                    ImdbId = s.ImdbId,
                    Abbreviation = s.Abbreviation,
                    Seasons = s.Seasons.Select(se => new SeasonVM(se.SeasonId, se.Number, se.Episodes)).ToArray(),
                    TranslatedName = _titleSynopsisLocalizer[s.TitleResource].Value,
                    Synopsis = _titleSynopsisLocalizer[s.SynopsisResource].Value,
                    Timeline = s.TimelineId
                })
                .FirstOrDefaultAsync();

            if (serie == null)
            {
                var errors = new Dictionary<string, IEnumerable<string>>()
                {
                    { "id", [_localizer["NotFound"]] }
                };
                throw new AppException(_localizer["NotFound"], errors, HttpStatusCode.NotFound);
            }
            return serie;
        }

        public async Task<SerieVM> Create(CreateSerieDto dto)
        {
            var validator = new CreateSerieValidation(_localizer, _context);
            var validation = await validator.ValidateAsync(dto);
            if (!validation.IsValid)
                throw new AppException(_localizer["OneOrMoreValidationErrorsOccurred"], validation.Errors);

            var languageIso = RegexHelper.RemoveSpecialCharacters(dto.OriginalLanguageIso);

            var newSerie = new Serie()
            {
                Abbreviation = dto.Abbreviation,
                ImdbId = dto.ImdbId,
                OriginalLanguageId = (short)Enum.Parse<LanguageEnum>(languageIso, true).GetHashCode(),
                OriginalName = dto.OriginalName.Trim(),
                SynopsisResource = dto.SynopsisResource,
                TitleResource = dto.TitleResource,
                TimelineId = (byte)dto.TimelineId.GetHashCode(),
                TmdbId = dto.TmdbId,
                Seasons = []
            };

            Parallel.ForEach(dto.Seasons, new ParallelOptions(), s =>
            {
                var newSeason = new Season()
                {
                    Number = s.Number,
                    Episodes = []
                };

                Parallel.ForEach(s.Episodes, new ParallelOptions(), episode =>
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
                newSerie.Seasons.Add(newSeason);
            });

            await _context.Serie.AddAsync(newSerie);
            await _context.SaveChangesAsync();

            var serieSaved = await _context.Serie.AsNoTracking()
                .Include(s => s.Seasons).ThenInclude(s => s.Episodes)
                .Include(s => s.Language)
                .OrderBy(s => s.SerieId)
                .LastAsync();

            return new SerieVM()
            {
                ID = serieSaved.SerieId,
                Abbreviation = serieSaved.Abbreviation,
                ImdbId = serieSaved.ImdbId,
                OriginalLanguage = serieSaved.Language.CodeISO,
                OriginalName = serieSaved.OriginalName,
                Seasons = serieSaved.Seasons.Select(se => new SeasonVM(se.SeasonId, se.Number, [.. se.Episodes])).ToList(),
                Timeline = serieSaved.TimelineId,
                TranslatedName = _titleSynopsisLocalizer[serieSaved.TitleResource],
                Synopsis = _titleSynopsisLocalizer[serieSaved.SynopsisResource]
            };
        }

        public async Task Update(short id, UpdateSerieDto dto)
        {
            var dtoValidation = new UpdateSerieValidation(_localizer);
            var validation = dtoValidation.Validate(dto);
            if (!validation.IsValid)
                throw new AppException(_localizer["OneOrMoreValidationErrorsOccurred"], validation.Errors);

            var serie = await _context.Serie.Where(s => s.SerieId.Equals(id)).FirstOrDefaultAsync()
                ?? throw new AppException(_localizer["NotFound"].Value, HttpStatusCode.NotFound);

            serie.Abbreviation = string.IsNullOrWhiteSpace(dto.Abbreviation) ? serie.Abbreviation : dto.Abbreviation;
            serie.ImdbId = string.IsNullOrWhiteSpace(dto.ImdbId) ? serie.ImdbId : dto.ImdbId.Trim();
            serie.OriginalName = string.IsNullOrWhiteSpace(dto.OriginalName) ? serie.OriginalName : dto.OriginalName.Trim();
            serie.TimelineId = (byte?)dto.TimelineId?.GetHashCode() ?? serie.TimelineId;
            serie.TmdbId = dto.TmdbId ?? serie.TmdbId;

            await _context.SaveChangesAsync();
        }
    }
}
