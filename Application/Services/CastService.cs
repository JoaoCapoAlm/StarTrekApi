using Application.Configurations;
using Application.Data;
using Application.Data.ViewModel;
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

        public async Task<IEnumerable<CastVM>> GetCastList(byte page = 0, byte pageSize = 100)
        {
            pageSize = pageSize > 100 ? (byte)100 : pageSize;

            return await _context.Cast
                .AsNoTracking()
                .OrderBy(c => c.CastId)
                .Skip(page * pageSize)
                .Take(pageSize)
                .Select(c => new CastVM {
                    Id = c.CastId,
                    Name = c.Name,
                    BirthDate = c.BirthDate,
                    DeathDate = c.DeathDate,
                    Country = _localizer[c.Country.ResourceName].Value
                })
                .ToArrayAsync()
                ?? throw new ExceptionNotFound(_localizer["NotFound"].Value);
        }

        public async Task<CastVM> GetCastById(int castId)
        {
            if (castId <= 0)
                throw new ArgumentException(_localizer["InvalidId"].Value);

            return await _context.Cast
                .AsNoTracking()
                .Where(c => c.CastId == castId)
                .Select(c => new CastVM {
                    Id = c.CastId,
                    Name = c.Name,
                    BirthDate = c.BirthDate,
                    DeathDate = c.DeathDate,
                    Country = _localizer[c.Country.ResourceName].Value
                })
                .FirstOrDefaultAsync()
                ?? throw new ExceptionNotFound(_localizer["NotFound"].Value);
        }
    }
}
