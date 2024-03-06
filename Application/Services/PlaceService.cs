using CrossCutting.Resources;
using Domain;
using Domain.Interfaces;
using Domain.ViewModel;
using Microsoft.Extensions.Localization;

namespace Application.Services
{
    public class PlaceService : IPlaceService
    {
        private readonly StarTrekContext _context;
        private readonly IStringLocalizer<Messages> _localizer;
        public PlaceService(StarTrekContext context, IStringLocalizer<Messages> localizer)
        {
            _context = context;
            _localizer = localizer;
        }

        public async Task<PlaceVM> GetById(short id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<PlaceVM>> GetList(byte page, byte pageSize)
        {
            throw new NotImplementedException();
        }
    }
}
