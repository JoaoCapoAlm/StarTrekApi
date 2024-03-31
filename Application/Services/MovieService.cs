using System.Net;
using AutoMapper;
using CrossCutting.Enums;
using CrossCutting.Exceptions;
using CrossCutting.Extensions;
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
    public class MovieService : IMovieService
    {
        private readonly StarTrekContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<Messages> _localizer;
        private readonly IStringLocalizer<TitleSynopsis> _titleSynopsisLocalizer;

        public MovieService(StarTrekContext context,
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

        public async Task<IEnumerable<MovieVM>> GetList(byte page, byte pageSize)
        {
            pageSize = (byte)(pageSize > 100 ? 100 : pageSize);

            var list = await _context.Movie
                .AsNoTracking()
                .OrderBy(m => m.MovieId)
                .Skip(page * pageSize)
                .Take(pageSize)
                .Include(x => x.Languages)
                .Include(x => x.Timeline)
                .Select(x => _mapper.Map<MovieVM>(x))
                .ToArrayAsync()
                ?? [];

            foreach (var movie in list)
            {
                movie.Synopsis = _titleSynopsisLocalizer[movie.Synopsis];
                movie.OriginalName = _titleSynopsisLocalizer[movie.OriginalName];
            }

            return list;
        }
        public async Task<MovieVM> GetById(short id)
        {
            if (id.Equals(byte.MinValue))
            {
                var errors = new Dictionary<string, IEnumerable<string>>()
                {
                    { "id", [_localizer["invalid"].Value] }
                };
                throw new AppException(_localizer["InvalidId"].Value, errors);
            }

            var movie = await _context.Movie
                .AsNoTracking()
                .Include(x => x.Languages)
                .Include(x => x.Timeline)
                .Where(m => m.MovieId.Equals(id))
                .Select(x => _mapper.Map<MovieVM>(x))
                .FirstOrDefaultAsync();

            if (movie == null)
            {
                var errors = new Dictionary<string, IEnumerable<string>>()
                {
                    { "id", [_localizer["NotFound"]] }
                };
                throw new AppException(_localizer["NotFound"], errors, HttpStatusCode.NotFound);
            }

            movie.TranslatedName = _titleSynopsisLocalizer[movie.TranslatedName];
            movie.Synopsis = _titleSynopsisLocalizer[movie.Synopsis];

            return movie;
        }
        public async Task<MovieVM> Create(CreateMovieDto dto)
        {
            var dtoValidation = new CreateMovieValidation(_localizer);
            dtoValidation.ValidateAndThrowStarTrek(dto, _localizer["OneOrMoreValidationErrorsOccurred"]);

            var checkExists = await _context.Movie.AsNoTracking()
                .Where(m => m.ImdbId.Equals(dto.ImdbId) || m.TmdbId.Equals(dto.TmdbId))
                .AnyAsync();

            if (checkExists)
            {
                var errors = new Dictionary<string, IEnumerable<string>>
                {
                    { "ImdbId/TmdbId", [_localizer["ImdbOrTmdbIdAlreadyRegistered"].Value] }
                };
                throw new AppException(_localizer["NotCreated"].Value, errors);
            }

            var movie = _mapper.Map<Movie>(dto);

            await _context.AddAsync(movie);
            await _context.SaveChangesAsync();

            var vm = _mapper.Map<MovieVM>(movie);

            vm.TranslatedName = _titleSynopsisLocalizer[vm.TranslatedName];
            vm.Synopsis = _titleSynopsisLocalizer[vm.Synopsis];

            return vm;
        }
        public async Task Update(short id, UpdateMovieDto dto)
        {
            var dtoValidation = new UpdateMovieValidation(_localizer, _context);
            var validation = await dtoValidation.ValidateAsync(dto);
            if (!validation.IsValid)
                throw new AppException(_localizer["OneOrMoreValidationErrorsOccurred"], validation.Errors);

            if (id <= 0)
            {
                var errors = new Dictionary<string, IEnumerable<string>>
                {
                    { "id", [_localizer["MustBeGreaterThanZero"]] }
                };
                throw new AppException(_localizer["OneOrMoreValidationErrorsOccurred"], errors);
            }

            var languageIso = RegexHelper.RemoveSpecialCharacters(dto.OriginalLanguageIso ?? string.Empty);
            LanguageEnum? languageParsed = null;
            if (!string.IsNullOrWhiteSpace(languageIso) && Enum.IsDefined(typeof(LanguageEnum), languageIso))
                languageParsed = Enum.Parse<LanguageEnum>(languageIso);

            var movie = await _context.Movie
                .Where(m => m.MovieId.Equals(id))
                .FirstOrDefaultAsync()
                ?? throw new AppException(_localizer["NotFound"].Value, HttpStatusCode.NotFound);

            movie.ImdbId = string.IsNullOrWhiteSpace(dto.ImdbId) ? movie.ImdbId : dto.ImdbId;
            movie.OriginalLanguageId = languageParsed.HasValue ? (short)languageParsed.Value.GetHashCode() : movie.OriginalLanguageId;
            movie.OriginalName = string.IsNullOrWhiteSpace(dto.OriginalName) ? movie.OriginalName.Trim() : dto.OriginalName;
            movie.ReleaseDate = dto.ReleaseDate ?? movie.ReleaseDate;
            movie.SynopsisResource = string.IsNullOrEmpty(dto.SynopsisResource) ? movie.SynopsisResource : dto.SynopsisResource;
            movie.Time = dto.Time ?? movie.Time;
            movie.TimelineId = dto.TimelineId.HasValue ? (byte)dto.TimelineId.Value.GetHashCode() : movie.TimelineId;
            movie.TitleResource = string.IsNullOrEmpty(dto.TitleResource) ? movie.TitleResource : dto.TitleResource;
            movie.TmdbId = dto.TmdbId ?? movie.TmdbId;

            _context.Entry(movie).State = EntityState.Modified;
            await _context.SaveChangesAsync();

        }
    }
}
