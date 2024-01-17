﻿using Application.Configurations;
using Application.Data;
using Application.Data.ViewModel;
using Application.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Application.Services
{
    public class SerieService
    {
        private readonly StarTrekContext _context;
        private readonly IStringLocalizer<Messages> _localizer;
        public SerieService(StarTrekContext context, IStringLocalizer<Messages> localizer)
        {
            _context = context;
            _localizer = localizer;
        }

        public async Task<IEnumerable<SerieVM>> GetSeriesList(byte page, byte pageSize)
        {
            pageSize = pageSize == 0 ? (byte)100 : pageSize;

            var seriesList = await _context.Serie
                .AsNoTracking()
                .OrderBy(s => s.SerieId)
                .Skip(page * pageSize)
                .Take(pageSize)
                .Select(s => new SerieVM
                {
                    ID = s.SerieId,
                    OriginalName = s.OriginalName,
                    Language = s.Language.ResourceName,
                    ImdbId = s.ImdbId,
                    Abbreviation = s.Abbreviation,
                    Seasons = s.Seasons.Select(se => new SeasonVM
                    {
                        ID = se.SeasonId,
                        Number = se.Number,
                        Episodes = se.Episodes.Select(e => new EpisodeVM
                        {
                            ID = e.EpisodeId,
                            Number = e.Number,
                            RealeaseDate = e.RealeaseDate,
                            ImdbId = e.ImdbId,
                            StardateFrom = e.StardateFrom,
                            StardateTo = e.StardateTo,
                            Time = e.Time,
                            SeasonId = e.SeasonId
                        }).ToArray()
                    }).ToArray()
                })
                .ToArrayAsync();

            if (seriesList.Any())
                return seriesList;
            else
                throw new ExceptionNotFound(_localizer["NotFound"].Value);
        }

        public async Task<SerieVM> GetSerieById(byte serieId)
        {
            if (serieId <= 0)
                throw new ArgumentException(_localizer["InvalidId"].Value);

            return await _context.Serie
                .AsNoTracking()
                .Where(s => s.SerieId == serieId)
                .Select(s => new SerieVM
                {
                    ID = s.SerieId,
                    OriginalName = s.OriginalName,
                    Language = s.Language.ResourceName,
                    ImdbId = s.ImdbId,
                    Abbreviation = s.Abbreviation,
                    Seasons = s.Seasons.Select(se => new SeasonVM
                    {
                        ID = se.SeasonId,
                        Number = se.Number,
                        Episodes = se.Episodes.Select(e => new EpisodeVM
                        {
                            ID = e.EpisodeId,
                            Number = e.Number,
                            RealeaseDate = e.RealeaseDate,
                            ImdbId = e.ImdbId,
                            StardateFrom = e.StardateFrom,
                            StardateTo = e.StardateTo,
                            Time = e.Time,
                            SeasonId = e.SeasonId
                        }).ToArray()
                    }).ToArray()
                })
                .FirstOrDefaultAsync()
                ?? throw new ExceptionNotFound(_localizer["NotFound"].Value);
        }
    }
}