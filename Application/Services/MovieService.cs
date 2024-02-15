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
            int beginRange = page * pageSize;

            return await _context.Movie
                .AsNoTracking()
                .OrderBy(m => m.ReleaseDate)
                .Take(beginRange..(beginRange + pageSize))
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
                ?? throw new AppException(_localizer["NotFound"].Value, Enumerable.Empty<ErrorContent>(), HttpStatusCode.NotFound);
        }

        public async Task<MovieVM> GetMovieById(byte movieId)
        {
            if (movieId.Equals(byte.MinValue))
                throw new AppException(_localizer["InvalidId"].Value, new List<ErrorContent>() { new("id", _localizer["invalid"].Value) });

            return await _context.Movie
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
                )).FirstOrDefaultAsync()
                ?? throw new AppException(_localizer["NotFound"].Value, Enumerable.Empty<ErrorContent>(), HttpStatusCode.NotFound);
        }

        public async Task<MovieVM> CreateMovie(CreateMovieDto dto)
        {
            var checkExists = await _context.Movie.AsNoTracking()
                .Where(m => m.ImdbId.Equals(dto.ImdbId) || m.TmdbId.Equals(dto.TmdbId))
                .AnyAsync();
            if (checkExists)
                throw new AppException(_localizer["NotCreated"].Value, new List<ErrorContent>()
                {
                    new("ImdbId/TmdbId", _localizer["ImdbOrTmdbIdAlreadyRegistered"].Value)
                });

            var errors = new List<ErrorContent>();
            var resources = await _context.vwResourcesName.AsNoTracking().ToArrayAsync();

            var checkResourceAlreadyExists = resources
                .Where(m => m.SynopsisResource.Equals(dto.SynopsisResource) || m.TitleResource.Equals(dto.SynopsisResource))
                .Any();
            
            if (checkResourceAlreadyExists)
                errors.Add(new ErrorContent("SynopsisResource", _localizer["AlreadyExists"].Value));

            checkResourceAlreadyExists = resources
                .Where(m => m.SynopsisResource.Equals(dto.TitleResource) || m.TitleResource.Equals(dto.TitleResource))
                .Any();

            if (checkResourceAlreadyExists)
                errors.Add(new ErrorContent("TitleResource", _localizer["AlreadyExists"].Value));

            if (errors.Any())
                throw new AppException(_localizer["NotCreated"].Value, errors);

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
            var errors = new List<ErrorContent>();
            if (id <= 0)
                errors.Add(new ErrorContent("id", _localizer["MustBeGreaterThanZero"].Value));

            var languageIso = RegexHelper.RemoveSpecialCharacters(dto.OriginalLanguageIso ?? string.Empty);
            LanguageEnum? languageParsed = null;
            if (!string.IsNullOrWhiteSpace(languageIso) && Enum.IsDefined(typeof(LanguageEnum), languageIso))
                languageParsed = Enum.Parse<LanguageEnum>(languageIso);

            var resources = await _context.vwResourcesName.AsNoTracking().ToArrayAsync();

            var checkResourcesAlreadyExists = resources
                .Where(m => m.MovieId != id && (m.SynopsisResource.Equals(dto.SynopsisResource) || m.TitleResource.Equals(dto.SynopsisResource)))
                .Any();

            if (checkResourcesAlreadyExists)
                errors.Add(new ErrorContent("SynopsisResource", _localizer["AlreadyExists"].Value));

            checkResourcesAlreadyExists = resources
                .Where(m => m.MovieId != id && (m.SynopsisResource.Equals(dto.TitleResource) || m.TitleResource.Equals(dto.TitleResource)))
                .Any();
            if (checkResourcesAlreadyExists)
                errors.Add(new ErrorContent("TitleResource", _localizer["AlreadyExists"].Value));

            if (errors.Any())
                throw new AppException("Existem erros nos dados informados.", errors); // TODO traduzir

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
                        .SetProperty(m => m.OriginalName, m => string.IsNullOrWhiteSpace(dto.OriginalName.Trim()) ? m.OriginalName : dto.OriginalName)
                        .SetProperty(m => m.ReleaseDate, m => dto.ReleaseDate ?? m.ReleaseDate)
                        .SetProperty(m => m.SynopsisResource, m => string.IsNullOrEmpty(dto.SynopsisResource) ? m.SynopsisResource : dto.SynopsisResource)
                        .SetProperty(m => m.Time, m => dto.Time ?? m.Time)
                        .SetProperty(m => m.TimelineId, m => timeline)
                        .SetProperty(m => m.TitleResource, m => string.IsNullOrEmpty(dto.TitleResource) ? m.TitleResource : dto.SynopsisResource)
                        .SetProperty(m => m.TmdbId, m => dto.TmdbId ?? m.TmdbId)
                );

            if (qtdMoviesUpdated.Equals(0))
                throw new AppException(_localizer["NotFound"].Value, Enumerable.Empty<ErrorContent>(), HttpStatusCode.NotFound);
        }
    }
}
