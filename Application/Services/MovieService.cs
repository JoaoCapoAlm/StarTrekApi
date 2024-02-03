using Application.Configurations;
using Application.Data;
using Application.Data.Enum;
using Application.Data.ViewModel;
using Application.Helpers;
using Application.Model;
using Application.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

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
                ?? throw new ExceptionNotFound(_localizer["NotFound"].Value);
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
                ?? throw new ExceptionNotFound(_localizer["NotFound"].Value);
        }

        public async Task<MovieVM> CreateMovie(CreateMovieDto dto)
        {
            var checkExists = await _context.Movie.AsNoTracking()
                .Where(m => m.ImdbId.Equals(dto.ImdbId) || m.TmdbId.Equals(dto.TmdbId))
                .AnyAsync();
            if (checkExists)
                throw new ArgumentException(_localizer["AlreadyExists"].Value);

            var errors = new List<string>();
            if (string.IsNullOrWhiteSpace(dto.SynopsisResource))
                errors.Add($"SynopsisResource: {_localizer["Required"].Value}");
            else if (dto.SynopsisResource.Contains(' '))
                errors.Add($"SynopsisResource: {_localizer["Invalid"].Value} - {_localizer["CannotContainSpace"].Value}");

            if (dto.Time <= 0)
                errors.Add($"Time: {_localizer["Invalid"].Value}");

            if (!string.IsNullOrWhiteSpace(dto.ImdbId) && !dto.ImdbId.StartsWith("tt"))
                errors.Add($"ImdbId: {_localizer["Invalid"].Value}");

            if (!Enum.IsDefined(typeof(TimelineEnum), dto.TimelineId))
                errors.Add($"TimelineId: {_localizer["Invalid"].Value}");

            if(dto.ReleaseDate > DateOnly.FromDateTime(DateTime.Today))
                errors.Add($"ReleaseDate: {_localizer["Invalid"].Value}");

            var languageIso = RegexHelper.RemoveSpecialCharacters(dto.OriginalLanguageIso);

            if (!Enum.IsDefined(typeof(LanguageEnum), languageIso))
                errors.Add($"OriginalLanguageIso: {_localizer["Invalid"].Value}");

            var checkSynopsisNameAlreadyExists = await _context.Movie.AsNoTracking()
                .Where(m => m.SynopsisResource.Equals(dto.SynopsisResource))
                .AnyAsync();

            if (checkSynopsisNameAlreadyExists)
                errors.Add($"SynopsisResource: {_localizer["AlreadyExists"].Value}");
            else
            {
                checkSynopsisNameAlreadyExists = await _context.Serie.AsNoTracking()
                    .Where(m => m.SynopsisResource.Equals(dto.SynopsisResource))
                    .AnyAsync();
                
                if (checkSynopsisNameAlreadyExists)
                    errors.Add($"SynopsisResource: {_localizer["AlreadyExists"].Value}");
            }

            if (errors.Any())
                throw new ArgumentException(StringHelper.ErrorListToString(errors));

            var movie = new Movie()
            {
                ImdbId = dto.ImdbId,
                OriginalLanguageId = (short)Enum.Parse(typeof(LanguageEnum), languageIso).GetHashCode(),
                OriginalName = dto.OriginalName,
                ReleaseDate = dto.ReleaseDate,
                TimelineId = dto.TimelineId,
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

        public async Task UpdateMovie(byte movieId, UpdateMovieDto dto)
        {
            var errors = new List<string>();
            var checkExists = await _context.Movie.AsNoTracking()
                .Where(m => m.MovieId.Equals(movieId))
                .AnyAsync();
            if (!checkExists)
                throw new ExceptionNotFound(_localizer["NotFound"].Value);

            if (!string.IsNullOrWhiteSpace(dto.SynopsisResource) && dto.SynopsisResource.Contains(' '))
                errors.Add($"SynopsisResource: {_localizer["Invalid"].Value} - {_localizer["CannotContainSpace"].Value}");

            if (dto.Time.HasValue && dto.Time.Value <= 0)
                errors.Add($"Time: {_localizer["Invalid"].Value}");

            if (!string.IsNullOrWhiteSpace(dto.ImdbId) && !dto.ImdbId.StartsWith("tt"))
                errors.Add($"ImdbId: {_localizer["Invalid"].Value}");

            if (dto.TimelineId.HasValue && !Enum.IsDefined(typeof(TimelineEnum), dto.TimelineId.Value))
                errors.Add($"TimelineId: {_localizer["Invalid"].Value}");

            if (dto.ReleaseDate.HasValue && dto.ReleaseDate.Value > DateOnly.FromDateTime(DateTime.Today))
                errors.Add($"ReleaseDate: {_localizer["Invalid"].Value}");

            var languageIso = RegexHelper.RemoveSpecialCharacters(dto.OriginalLanguageIso ?? string.Empty);
            LanguageEnum? languageParsed = null;
            if (!string.IsNullOrWhiteSpace(languageIso))
            {
                if (Enum.IsDefined(typeof(LanguageEnum), languageIso))
                    languageParsed = Enum.Parse<LanguageEnum>(languageIso);
                else
                    errors.Add($"OriginalLanguageIso: {_localizer["Invalid"].Value}");
            }

            var checkSynopsisNameAlreadyExists = await _context.Movie.AsNoTracking()
                .Where(m => m.MovieId != movieId && m.SynopsisResource.Equals(dto.SynopsisResource))
                .AnyAsync();

            if (checkSynopsisNameAlreadyExists)
                errors.Add($"SynopsisResource: {_localizer["AlreadyExists"].Value}");
            else
            {
                checkSynopsisNameAlreadyExists = await _context.Serie.AsNoTracking()
                    .Where(m => m.SynopsisResource.Equals(dto.SynopsisResource))
                    .AnyAsync();

                if (checkSynopsisNameAlreadyExists)
                    errors.Add($"SynopsisResource: {_localizer["AlreadyExists"].Value}");
            }

            if (errors.Any())
                throw new ArgumentException(StringHelper.ErrorListToString(errors));

            var qtdMoviesUpdated = await _context.Movie.AsNoTracking()
                .Where(m => m.MovieId.Equals(movieId))
                .ExecuteUpdateAsync(s =>
                    s.SetProperty(m => m.ImdbId, m => string.IsNullOrWhiteSpace(dto.ImdbId) ? m.ImdbId : dto.ImdbId)
                        .SetProperty(m => m.OriginalLanguageId, m => languageParsed.HasValue ? languageParsed.Value.GetHashCode() : m.OriginalLanguageId)
                        .SetProperty(m => m.OriginalName, m => string.IsNullOrWhiteSpace(dto.OriginalName) ? m.OriginalName : dto.OriginalName)
                        .SetProperty(m => m.ReleaseDate, m => dto.ReleaseDate ?? m.ReleaseDate)
                        .SetProperty(m => m.SynopsisResource, m => string.IsNullOrWhiteSpace(dto.SynopsisResource) ? m.SynopsisResource : dto.SynopsisResource)
                        .SetProperty(m => m.Time, m => dto.Time ?? m.Time)
                        .SetProperty(m => m.TimelineId, m => dto.TimelineId ?? m.TimelineId)
                        .SetProperty(m => m.TmdbId, m => dto.TmdbId ?? m.TmdbId)
                );

            if (qtdMoviesUpdated.Equals(0))
                throw new ExceptionNotFound(_localizer["NotFound"].Value);
        }
    }
}
