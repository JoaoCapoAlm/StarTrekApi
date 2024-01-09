using System.Drawing;
using Application.Data;
using Application.Data.ViewModel;
using Application.Model;
using Application.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Application.Services
{
    public class CastService
    {
        private readonly StarTrekContext _context;
        private readonly IStringLocalizer<Messages> _localizer;

        public CastService(StarTrekContext context, IStringLocalizer<Messages> localizer)
        {
            _context = context;
            _localizer = localizer;
        }

        public async Task<IEnumerable<CastVM>> GetCasts(byte? page = 0, byte? pageSize = 100)
        {
            var pageNumber = (!page.HasValue || page.Value < 0) ? (byte)0 : page.Value;
            var size = (!pageSize.HasValue || pageSize.Value < 0) ? (byte)100 : pageSize.Value;

            return await _context.Cast
                .AsNoTracking()
                .OrderBy(c => c.CastId)
                .Skip(pageNumber * size)
                .Take(size)
                .Select(c => new CastVM {
                    Id = c.CastId,
                    Name = c.Name,
                    BirthDate = c.BirthDate,
                    DeathDate = c.DeathDate,
                    Country = _localizer[c.Country.ResourceName].Value
                })
                .ToArrayAsync();
        }

        public async Task<CastVM> GetCast(byte castId)
        {
            if (castId <= 0)
                return null;

            return await _context.Cast
                .AsNoTracking()
                .Where(c => c.CastId == castId)
                .Select(c => new CastVM
                {
                    Id = c.CastId,
                    Name = c.Name,
                    BirthDate = c.BirthDate,
                    DeathDate = c.DeathDate,
                    Country = _localizer[c.Country.ResourceName].Value
                })
                .FirstOrDefaultAsync();
        }
    }
}
