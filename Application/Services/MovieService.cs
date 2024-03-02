using System.Net;
using Application.Data.ViewModel;
using CrossCutting.Enums;
using CrossCutting.Exceptions;
using CrossCutting.Helpers;
using CrossCutting.Resources;
using Domain;
using Domain.Model;
using Domain.Validation;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Application.Services
{
    public class MovieService(StarTrekContext context,
        IStringLocalizer<Messages> localizer,
        IStringLocalizer<TitleSynopsis> titleSynopsisLocalizer)
    {
        private readonly StarTrekContext _context = context;
        private readonly IStringLocalizer<Messages> _localizer = localizer;
        private readonly IStringLocalizer<TitleSynopsis> _titleSynopsisLocalizer = titleSynopsisLocalizer;

        public async Task<IEnumerable<MovieVM>> GetMovieList(byte page = byte.MinValue, byte pageSize = 100)
        {
            pageSize = (byte)(pageSize > 100 ? 100 : pageSize);

            return await _context.Movie
                .AsNoTracking()
                .OrderBy(m => m.MovieId)
                .Skip(page * pageSize)
                .Take(pageSize)
                .Select(m => new MovieVM(
                    m.MovieId,
                    m.OriginalName,
                    _localizer[m.SynopsisResource].Value,
                    m.Languages.CodeISO,
                    m.Time,
                    m.ImdbId,
                    m.ReleaseDate,
                    m.TimelineId,
                    _localizer[m.TitleResource].Value
                )).ToArrayAsync()
                ?? Enumerable.Empty<MovieVM>();
        }

        public async Task<MovieVM> GetMovieById(byte movieId)
        {
            if (movieId.Equals(byte.MinValue))
            {
                var errors = new Dictionary<string, IEnumerable<string>>()
                {
                    { "id", [_localizer["invalid"].Value] }
                };
                throw new AppException(_localizer["InvalidId"].Value, errors);
            }

            var movie = await _context.Movie
                .AsNoTracking()
                .Where(m => m.MovieId == movieId)
                .Select(m => new MovieVM(
                    m.MovieId,
                    m.OriginalName,
                    _titleSynopsisLocalizer[m.SynopsisResource].Value,
                    m.Languages.CodeISO,
                    m.Time,
                    m.ImdbId,
                    m.ReleaseDate,
                    m.TimelineId,
                    _titleSynopsisLocalizer[m.TitleResource].Value
                )).FirstOrDefaultAsync();

            if (movie == null)
            {
                var errors = new Dictionary<string, IEnumerable<string>>()
                {
                    { "id", [_localizer["NotFound"]] }
                };
                throw new AppException(_localizer["NotFound"], errors, HttpStatusCode.NotFound);
            }

            return movie;
        }

        public async Task<MovieVM> CreateMovie(CreateMovieDto dto)
        {
            var dtoValidation = new CreateMovieValidation(_localizer);
            var validation = dtoValidation.Validate(dto);
            if (!validation.IsValid)
                throw new AppException(_localizer["OneOrMoreValidationErrorsOccurred"], validation.Errors);
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

            var languageIso = RegexHelper.RemoveSpecialCharacters(dto.OriginalLanguageIso);

            var movie = new Movie()
            {
                ImdbId = dto.ImdbId,
                OriginalLanguageId = (short)Enum.Parse<LanguageEnum>(languageIso, true).GetHashCode(),
                OriginalName = dto.OriginalName,
                ReleaseDate = dto.ReleaseDate,
                TimelineId = (byte)dto.TimelineId,
                TitleResource = dto.TitleResource,
                TmdbId = dto.TmdbId,
                SynopsisResource = dto.SynopsisResource,
                Time = dto.Time
            };

            await _context.AddAsync(movie);
            await _context.SaveChangesAsync();

            return await _context.Movie.AsNoTracking()
                .OrderBy(m => m.MovieId)
                .Select(m => new MovieVM(m.MovieId,
                    m.OriginalName,
                    _titleSynopsisLocalizer[m.SynopsisResource].Value,
                    m.Languages.CodeISO,
                    m.Time,
                    m.ImdbId,
                    m.ReleaseDate,
                    m.TimelineId,
                    _titleSynopsisLocalizer[m.TitleResource].Value)
                ).LastAsync();
        }

        public async Task UpdateMovie(short id, UpdateMovieDto dto)
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
