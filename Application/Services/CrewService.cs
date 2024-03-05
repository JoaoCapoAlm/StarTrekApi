using Application.Data.ViewModel;
using CrossCutting.Exceptions;
using CrossCutting.Resources;
using Domain;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Application.Services
{
    public class CrewService : ICrewService
    {
        private readonly StarTrekContext _context;
        private readonly IStringLocalizer<Messages> _localizer;

        public CrewService(StarTrekContext context, IStringLocalizer<Messages> localizer)
        {
            _context = context;
            _localizer = localizer;
        }

        public async Task<IEnumerable<CrewVM>> GetList(byte page = 0, byte pageSize = 100)
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

        public async Task<CrewVM> GetById(int crewId)
        {
            if (crewId <= 0)
            {
                var error = new Dictionary<string, IEnumerable<string>>
                {
                    { "ID", [_localizer["InvalidId"].Value] }
                };
                throw new AppException(_localizer["InvalidId"].Value, error);
            }

            var crew = await _context.Crew
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
                .FirstOrDefaultAsync();

            if (crew == null)
            {
                var error = new Dictionary<string, IEnumerable<string>>()
                {
                    { "id", [_localizer["NotFound"]] }
                };
                throw new AppException(_localizer["NotFound"].Value, error, System.Net.HttpStatusCode.NotFound);
            }

            return crew;
        }
    }
}
