using Application.Data;
using Microsoft.EntityFrameworkCore;

namespace Application.Repositories
{
    public class ViewsRepository
    {
        private readonly StarTrekContext _context;
        public ViewsRepository(StarTrekContext context)
        {
            _context = context;
        }
        public async Task<bool> CheckResourceExists(string resource, string id = null, CancellationToken cancellationToken = default)
        {
            var result =  await _context.vwResourcesName.AsNoTracking()
                .Where(r => (r.SynopsisResource.Equals(resource) || r.TitleResource.Equals(resource))
                    && (string.IsNullOrWhiteSpace(id) || r.Id == id))
                .AnyAsync(cancellationToken);

            return result;
        }
    }
}