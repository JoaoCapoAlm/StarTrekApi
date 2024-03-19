using System.Data;
using System.Linq.Expressions;
using System.Net;
using AutoMapper;
using ClosedXML.Excel;
using CrossCutting.AppModel;
using CrossCutting.Exceptions;
using CrossCutting.Helpers;
using CrossCutting.Resources;
using Domain;
using Domain.DTOs;
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
        )
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
            _titleSynopsisLocalizer = titleSynopsisLocalizer;
        }

        public async Task<IEnumerable<SerieVM>> GetList(byte page, byte pageSize, string name)
        {
            pageSize = pageSize == 0 ? (byte)100 : pageSize;

            var list = await _context.Serie
                .AsNoTracking()
                .Include(x => x.Seasons).ThenInclude(x => x.Episodes)
                .Include(x => x.Language)
                .Include(x => x.Timeline)
                .AsSplitQuery()
                .OrderBy(s => s.SerieId)
                .Skip(page * pageSize)
                .Take(pageSize)
                .Select(x => _mapper.Map<SerieVM>(x))
                .ToArrayAsync()
                ?? [];

            Parallel.ForEach(list, serie =>
            {
                serie.TranslatedName = _titleSynopsisLocalizer[serie.TranslatedName];
                serie.Synopsis = _titleSynopsisLocalizer[serie.Synopsis];
                serie.OriginalLanguage.Name = _localizer[serie.OriginalLanguage.Name];

                Parallel.ForEach(serie.Seasons, season =>
                {
                    Parallel.ForEach(season.Episodes, episode =>
                    {
                        episode.Synopsis = _titleSynopsisLocalizer[episode.Synopsis];
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
                .Include(x => x.Timeline)
                .Include(x => x.Seasons).ThenInclude(x => x.Episodes)
                .Include(x => x.Language)
                .Where(s => s.SerieId.Equals(id))
                .Select(x => _mapper.Map<SerieVM>(x))
                .FirstOrDefaultAsync();

            if (serie == null)
            {
                var errors = new Dictionary<string, IEnumerable<string>>()
                {
                    { "id", [_localizer["NotFound"]] }
                };
                throw new AppException(_localizer["NotFound"], errors, HttpStatusCode.NotFound);
            }

            serie.TranslatedName = _titleSynopsisLocalizer[serie.TranslatedName];
            serie.Synopsis = _titleSynopsisLocalizer[serie.Synopsis];
            serie.OriginalLanguage.Name = _localizer[serie.OriginalLanguage.Name];

            Parallel.ForEach(serie.Seasons, season =>
            {
                Parallel.ForEach(season.Episodes, episode =>
                {
                    episode.Synopsis = _titleSynopsisLocalizer[episode.Synopsis];
                    episode.TranslatedTitle = _titleSynopsisLocalizer[episode.TranslatedTitle];
                });
            });

            return serie;
        }

        public async Task<SerieVM> Create(CreateSerieDto dto)
        {
            var validator = new CreateSerieValidation(_localizer, _context);
            var validation = await validator.ValidateAsync(dto);
            if (!validation.IsValid)
                throw new AppException(_localizer["OneOrMoreValidationErrorsOccurred"], validation.Errors);

            var newSerie = _mapper.Map<Serie>(dto);

            await _context.Serie.AddAsync(newSerie);
            await _context.SaveChangesAsync();

            var vm = _mapper.Map<SerieVM>(newSerie);

            vm.OriginalLanguage = await _context.Language.AsNoTracking()
                .Where(x => x.LanguageId.Equals(newSerie.OriginalLanguageId))
                .Select(x => _mapper.Map<LanguageVM>(x))
                .FirstAsync();

            vm.Synopsis = _titleSynopsisLocalizer[vm.Synopsis];
            vm.TranslatedName = _titleSynopsisLocalizer[vm.TranslatedName];

            return vm;
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

        public async Task<FileContent> Export()
        {
            var series = await _context.Serie.AsNoTracking()
                .Include(x => x.Timeline)
                .Include(x => x.Language)
                .Include(x => x.Seasons).ThenInclude(x => x.Episodes)
                .OrderBy(x => x.SerieId)
                .Select(x => _mapper.Map<SerieVM>(x))
                .ToArrayAsync();

            var typeNumber = XLDataType.Number.GetType();
            var dataTable = new DataTable(_localizer["Serie"]);
            #region Header
            dataTable.Columns.Add("ID", typeNumber);
            dataTable.Columns.Add(_localizer["TranslatedName"]);
            dataTable.Columns.Add(_localizer["OriginalName"]);
            dataTable.Columns.Add(_localizer["Abbreviation"]);
            dataTable.Columns.Add(_localizer["Timeline"]);
            dataTable.Columns.Add(_localizer["OriginalLanguage"]);
            dataTable.Columns.Add("IMDB ID");
            dataTable.Columns.Add(_localizer["Synopsis"]);

            var dataTableSeason = new DataTable(_localizer["Season"]);
            dataTableSeason.Columns.Add("ID", typeNumber);
            dataTableSeason.Columns.Add(_localizer["SerieID"], typeNumber);
            dataTableSeason.Columns.Add(_localizer["Number"], typeNumber);

            var dataTableEpisode = new DataTable(_localizer["Episodes"]);
            dataTableEpisode.Columns.Add("ID", typeNumber);
            dataTableEpisode.Columns.Add(_localizer["Number"], typeNumber);
            dataTableEpisode.Columns.Add(_localizer["SeasonID"], typeNumber);
            dataTableEpisode.Columns.Add(_localizer["TranslatedName"]);
            dataTableEpisode.Columns.Add(_localizer["Time"], typeNumber);
            dataTableEpisode.Columns.Add(_localizer["RealeaseDate"], XLDataType.DateTime.GetType());
            dataTableEpisode.Columns.Add(_localizer["StardateFrom"]);
            dataTableEpisode.Columns.Add(_localizer["StardateTo"]);
            dataTableEpisode.Columns.Add(_localizer["Synopsis"]);
            dataTableEpisode.Columns.Add("IMDB ID");
            #endregion
            #region Body
            DataRow row;
            List<SeasonVM> seasonsOrdened;
            List<EpisodeVM> episodesOrdened;
            foreach (var serie in series)
            {
                row = dataTable.NewRow();
                row.ItemArray = [
                    serie.ID,
                    _titleSynopsisLocalizer[serie.TranslatedName],
                    serie.OriginalName,
                    serie.Abbreviation,
                    $"{serie.Timeline.ID} - {serie.Timeline.Name}",
                    $"{serie.OriginalLanguage.CodeISO} - {_localizer[serie.OriginalLanguage.Name]}",
                    serie.ImdbId,
                    _titleSynopsisLocalizer[serie.Synopsis]
                ];
                dataTable.Rows.Add(row);

                seasonsOrdened = [.. serie.Seasons.OrderBy(x => x.Number)];
                foreach (var season in seasonsOrdened)
                {
                    row = dataTableSeason.NewRow();
                    row.ItemArray = [
                        season.ID,
                        serie.ID,
                        season.Number
                    ];

                    dataTableSeason.Rows.Add(row);

                    episodesOrdened = [.. season.Episodes.OrderBy(x => x.Number)];
                    foreach (var episode in episodesOrdened)
                    {
                        row = dataTableEpisode.NewRow();
                        row.ItemArray = [
                            episode.ID,
                            episode.Number,
                            season.ID,
                            _titleSynopsisLocalizer[episode.TranslatedTitle],
                            episode.Time,
                            episode.RealeaseDate,
                            episode.StardateFrom,
                            episode.StardateTo,
                            _titleSynopsisLocalizer[episode.Synopsis],
                            episode.ImdbId
                        ];

                        dataTableEpisode.Rows.Add(row);
                    }
                }
            }
            #endregion

            return ExcelHelper.GenerateExcel([dataTable, dataTableSeason, dataTableEpisode], _localizer["Serie"]);
        }
    }
}
