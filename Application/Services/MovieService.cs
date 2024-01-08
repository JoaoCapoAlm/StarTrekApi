using Application.Data;
using Application.Model;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class MovieService
    {
        private readonly StarTrekContext _context;
        public MovieService(StarTrekContext context)
        {
            _context = context;
        }

        public async Task<IList<Movie>> GetMovies(byte page = 0, byte pageSize = 100)
        {
            page = (byte)(page < 0 ? 0 : page);
            pageSize = (byte)(pageSize > 100 ? 100 : pageSize);
            pageSize = (byte)(pageSize < 0 ? 0 : pageSize);

            return await _context.Movie
                .AsNoTracking()
                .OrderBy(m => m.ReleaseDate)
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Movie> GetMovie(byte movieId)
        {
            if (movieId < 1)
                return null;

            return await _context.Movie.AsNoTracking().Where(m => m.MovieId == movieId).FirstOrDefaultAsync();
        }
    }
}
