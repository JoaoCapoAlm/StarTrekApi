using System.Net;
using Application.Configurations;
using Application.Data;
using Application.Data.Enum;
using Application.Data.ViewModel;
using Application.Helpers;
using Application.Model;
using Application.Resources;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using static Application.Middleware.AppMiddleware;

namespace Application.Services
{
    public class MovieService(StarTrekContext context, IStringLocalizer<Messages> localizer)
    {
        private readonly StarTrekContext _context = context;
        private readonly IStringLocalizer<Messages> _localizer = localizer;

        public async Task<IEnumerable<MovieVM>> GetMovieList(byte page = byte.MinValue, byte pageSize = 100)
        {
            pageSize = (byte)(pageSize > 100 ? 100 : pageSize);

            return await _context.Movie
                .AsNoTracking()
                .OrderBy(m => m.ReleaseDate)
                .Skip(page * pageSize)
                .Take(pageSize)
                .Select(m => new MovieVM(
                    m.MovieId,
                    m.OriginalName,
                    m.SynopsisResource,
                    _localizer[m.Languages.ResourceName].Value,
                    m.Time,
                    m.ImdbId,
                    m.ReleaseDate,
                    m.TimelineId
                )).ToArrayAsync()
                ?? throw new AppException(_localizer["NotFound"].Value, Enumerable.Empty<ErrorContent>(), HttpStatusCode.NotFound);
        }

        public async Task<MovieVM> GetMovieById(byte movieId)
        {
            if (movieId < 1)
                throw new ArgumentException(_localizer["InvalidId"].Value);

            return await _context.Movie
                .AsNoTracking()
                .Where(m => m.MovieId == movieId)
                .Select(m => new MovieVM(
                    m.MovieId,
                    m.OriginalName,
                    m.SynopsisResource,
                    _localizer[m.Languages.ResourceName].Value,
                    m.Time,
                    m.ImdbId,
                    m.ReleaseDate,
                    m.TimelineId
                )).FirstOrDefaultAsync()
                ?? throw new AppException(_localizer["NotFound"].Value, Enumerable.Empty<ErrorContent>(), HttpStatusCode.NotFound);
        }

        public async Task<MovieVM> CreateMovie(CreateMovieDto dto)
        {
            var checkExists = await _context.Movie.AsNoTracking()
                .Where(m => m.ImdbId.Equals(dto.ImdbId) || m.TmdbId.Equals(dto.TmdbId))
                .AnyAsync();
            if (checkExists)
                throw new AppException(_localizer["AlreadyExists"].Value, new List<ErrorContent>()
                {
                    new("ImdbId/TmdbId", _localizer["AlreadyExists"].Value)
                });

            var errors = new List<ErrorContent>();
            var checkSynopsisNameAlreadyExists = await _context.Movie.AsNoTracking()
                .Where(m => m.SynopsisResource.Equals(dto.SynopsisResource))
                .AnyAsync();

            if (checkSynopsisNameAlreadyExists)
                errors.Add(new ErrorContent("SynopsisResource", _localizer["AlreadyExists"].Value));
            else
            {
                checkSynopsisNameAlreadyExists = await _context.Serie.AsNoTracking()
                    .Where(m => m.SynopsisResource.Equals(dto.SynopsisResource))
                    .AnyAsync();

                if (checkSynopsisNameAlreadyExists)
                    errors.Add(new ErrorContent("SynopsisResource", _localizer["AlreadyExists"].Value));

                if (errors.Any())
                    throw new AppException("Não foi possível criar o filme", errors);
            }

            var languageIso = RegexHelper.RemoveSpecialCharacters(dto.OriginalLanguageIso);

            var movie = new Movie()
            {
                ImdbId = dto.ImdbId,
                OriginalLanguageId = (short)Enum.Parse<LanguageEnum>(languageIso, true).GetHashCode(),
                OriginalName = dto.OriginalName,
                ReleaseDate = dto.ReleaseDate,
                TimelineId = (byte)dto.TimelineId,
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
                    m.SynopsisResource,
                    m.Languages.CodeISO,
                    m.Time,
                    m.ImdbId,
                    m.ReleaseDate,
                    m.TimelineId)
                ).LastAsync();
        }

        public async Task UpdateMovie(byte id, UpdateMovieDto dto)
        {
            var languageIso = RegexHelper.RemoveSpecialCharacters(dto.OriginalLanguageIso ?? string.Empty);
            LanguageEnum? languageParsed = null;
            if (!string.IsNullOrWhiteSpace(languageIso) && Enum.IsDefined(typeof(LanguageEnum), languageIso))
                languageParsed = Enum.Parse<LanguageEnum>(languageIso);

            var checkSynopsisNameAlreadyExists = await _context.Movie.AsNoTracking()
                .Where(m => m.MovieId != id && m.SynopsisResource.Equals(dto.SynopsisResource))
                .AnyAsync();

            var errors = new List<ErrorContent>();
            if (checkSynopsisNameAlreadyExists)
                errors.Add(new ErrorContent("SynopsisResource", _localizer["AlreadyExists"].Value));
            else
            {
                checkSynopsisNameAlreadyExists = await _context.Serie.AsNoTracking()
                    .Where(m => m.SynopsisResource.Equals(dto.SynopsisResource))
                    .AnyAsync();

                if (checkSynopsisNameAlreadyExists)
                    errors.Add(new ErrorContent("SynopsisResource", _localizer["AlreadyExists"].Value));
            }

            if (errors.Any())
                throw new AppException("Existem erros nos dados", errors);

            var checkExists = await _context.Movie.AsNoTracking()
                .Where(m => m.MovieId.Equals(id))
                .FirstOrDefaultAsync()
                ?? throw new AppException(_localizer["NotFound"].Value, Enumerable.Empty<ErrorContent>(), HttpStatusCode.NotFound);

            var timeline = dto.TimelineId.HasValue ? (byte)dto.TimelineId.Value.GetHashCode() : checkExists.TimelineId;

            var qtdMoviesUpdated = await _context.Movie.AsNoTracking()
                .Where(m => m.MovieId.Equals(id))
                .ExecuteUpdateAsync(s =>
                    s.SetProperty(m => m.ImdbId, m => string.IsNullOrWhiteSpace(dto.ImdbId) ? m.ImdbId : dto.ImdbId)
                        .SetProperty(m => m.OriginalLanguageId, m => languageParsed.HasValue ? languageParsed.Value.GetHashCode() : m.OriginalLanguageId)
                        .SetProperty(m => m.OriginalName, m => string.IsNullOrWhiteSpace(dto.OriginalName) ? m.OriginalName : dto.OriginalName)
                        .SetProperty(m => m.ReleaseDate, m => dto.ReleaseDate ?? m.ReleaseDate)
                        .SetProperty(m => m.SynopsisResource, m => string.IsNullOrWhiteSpace(dto.SynopsisResource) ? m.SynopsisResource : dto.SynopsisResource)
                        .SetProperty(m => m.Time, m => dto.Time ?? m.Time)
                        .SetProperty(m => m.TimelineId, m => timeline)
                        .SetProperty(m => m.TmdbId, m => dto.TmdbId ?? m.TmdbId)
                );

            if (qtdMoviesUpdated.Equals(0))
                throw new AppException(_localizer["NotFound"].Value, Enumerable.Empty<ErrorContent>(), HttpStatusCode.NotFound);
        }
    }
}
