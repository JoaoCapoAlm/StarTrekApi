using Application.Data;
using Microsoft.EntityFrameworkCore;

namespace Application.Repositories
{
    public class SerieRepository
    {
        private readonly StarTrekContext _context;
        public SerieRepository(StarTrekContext context)
        {
            _context = context;
        }
        public async Task<bool> CheckImdbOrTmdbExists(string imdb, int? tmdb, CancellationToken cancellationToken)
        {
            return await _context.Serie.AsNoTracking()
                .Where(s => (imdb != string.Empty && s.ImdbId.Equals(imdb))
                    || (tmdb.HasValue && s.TmdbId.Equals(tmdb.Value))
                ).AnyAsync(cancellationToken);
        }
    }
}