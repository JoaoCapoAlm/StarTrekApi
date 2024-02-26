using Application.Configurations;
using Application.Data;
using Application.Data.ViewModel;
using Application.Resources;
using Domain;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using static Application.Middleware.AppMiddleware;

namespace Application.Services
{
    public class CrewService
    {
        private readonly StarTrekContext _context;
        private readonly IStringLocalizer<Messages> _localizer;

        public CrewService(StarTrekContext context, IStringLocalizer<Messages> localizer)
        {
            _context = context;
            _localizer = localizer;
        }

        public async Task<IEnumerable<CrewVM>> GetCrewList(byte page = 0, byte pageSize = 100)
        {
            pageSize = pageSize > 100 ? (byte)100 : pageSize;

            return await _context.Crew
                .AsNoTracking()
                .OrderBy(c => c.CrewId)
                .Skip(page * pageSize)
                .Take(pageSize)
                .Select(c => new CrewVM
                {
                    Id = c.CrewId,
                    Name = c.Name,
                    BirthDate = c.BirthDate,
                    DeathDate = c.DeathDate,
                    Country = _localizer[c.Country.ResourceName].Value
                })
                .ToArrayAsync()
                ?? throw new AppException(_localizer["NotFound"].Value, System.Net.HttpStatusCode.NotFound);
        }

        public async Task<CrewVM> GetCrewById(int crewId)
        {
            if (crewId <= 0)
            {
                var error = new Dictionary<string, IEnumerable<string>>
                {
                    { "ID", [_localizer["InvalidId"].Value] }
                };
                throw new AppException(_localizer["InvalidId"].Value, error);
            }

            return await _context.Crew
                .AsNoTracking()
                .Where(c => c.CrewId == crewId)
                .Select(c => new CrewVM
                {
                    Id = c.CrewId,
                    Name = c.Name,
                    BirthDate = c.BirthDate,
                    DeathDate = c.DeathDate,
                    Country = _localizer[c.Country.ResourceName].Value
                })
                .FirstOrDefaultAsync()
                ?? throw new AppException(_localizer["NotFound"].Value, System.Net.HttpStatusCode.NotFound);
        }
    }
}
