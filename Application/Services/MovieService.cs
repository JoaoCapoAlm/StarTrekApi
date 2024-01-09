using Application.Data;
using Application.Data.ViewModel;
using Application.Model;
using Application.Resources;
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

        public async Task<IEnumerable<MovieVM>> GetMovies(byte page = 0, byte pageSize = 100)
        {
            page = (byte)(page < 0 ? 0 : page);
            pageSize = (byte)(pageSize > 100 ? 100 : pageSize);
            pageSize = (byte)(pageSize < 0 ? 0 : pageSize);

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
                )).ToArrayAsync();
        }

        public async Task<MovieVM> GetMovie(byte movieId)
        {
            if (movieId < 1)
                return null;

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
                )).FirstOrDefaultAsync();
        }
    }
}
