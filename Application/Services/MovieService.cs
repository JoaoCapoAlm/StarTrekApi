using System.Text;
using Application.Configurations;
using Application.Data;
using Application.Data.Enum;
using Application.Data.ViewModel;
using Application.Model;
using Application.Resources;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Application.Services
{
    public class MovieService
    {
        private readonly StarTrekContext _context;
        private readonly IStringLocalizer<Messages> _localizer;
        public MovieService(StarTrekContext context, IStringLocalizer<Messages> localizer)
        {
            _context = context;
            _localizer = localizer;
        }

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
                    m.ReleaseDate
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
                    m.ReleaseDate
                )).FirstOrDefaultAsync()
                ?? throw new ExceptionNotFound(_localizer["NotFound"].Value);
        }

        public async Task<MovieVM> CreateMovie(CreateMovieDto dto)
        {
            var checkExists = await _context.Movie.Where(m => m.ImdbId == dto.ImdbId || m.TmdbId == dto.TmdbId).AnyAsync();
            if (checkExists)
                throw new ArgumentException(_localizer["AlreadyExists"].Value);

            var errors = new List<string>();
            try
            {
                if (string.IsNullOrWhiteSpace(dto.SynopsisResource))
                    errors.Add($"SynopsisResource: {_localizer["Required"].Value}");

                if (dto.Time <= 0)
                    errors.Add($"Time: {_localizer["Invalid"].Value}");

                if (!string.IsNullOrWhiteSpace(dto.ImdbId) && !dto.ImdbId.StartsWith("tt"))
                    errors.Add($"ImdbId: {_localizer["Invalid"].Value}");

                if (!Enum.IsDefined(typeof(TimelineEnum), dto.TimelineId))
                    errors.Add($"TimelineId: {_localizer["Invalid"].Value}");

                if(dto.ReleaseDate < DateOnly.FromDateTime(DateTime.Today))
                    errors.Add($"ReleaseDate: {_localizer["Invalid"].Value}");

                if (errors.Any())
                    throw new ArgumentException();

                var movie = new Movie()
                {
                    ImdbId = dto.ImdbId,
                    OriginalLanguageId = dto.OriginalLanguageId,
                    OriginalName = dto.OriginalName,
                    ReleaseDate = dto.ReleaseDate,
                    TimelineId = dto.TimelineId,
                    TmdbId = dto.TmdbId,
                    SynopsisResource = dto.SynopsisResource,
                    Time = dto.Time
                };

                await _context.AddAsync(movie);
                await _context.SaveChangesAsync();

                return await _context.Movie.Select(m => new MovieVM(m.MovieId,
                    m.OriginalName,
                    m.SynopsisResource,
                    m.Languages.CodeISO,
                    m.Time,
                    m.ImdbId,
                    m.ReleaseDate)).LastAsync();
            } catch (ArgumentException)
            {
                var message = new StringBuilder();
                errors.ForEach(m => message.Append($"- {m}{Environment.NewLine}"));
                throw new ArgumentException(message.ToString().Trim());
            }
        }
    }
}
