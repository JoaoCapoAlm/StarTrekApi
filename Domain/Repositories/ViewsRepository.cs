using Microsoft.EntityFrameworkCore;

namespace Domain.Repositories
{
    public class ViewsRepository
    {
        private readonly StarTrekContext _context;
        public ViewsRepository(StarTrekContext context)
        {
            _context = context;
        }
        public async Task<bool> CheckResourceExists(string resource, int? id = null, CancellationToken cancellationToken = default)
        {
            var result = await _context.vwResourcesName.AsNoTracking()
                .Where(r => r.Resource.Equals(resource)
                    && (!id.HasValue || r.Id.Equals(id)))
                .AnyAsync(cancellationToken);

            return result;
        }
    }
}