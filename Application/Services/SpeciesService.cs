using System.Linq.Expressions;
using AutoMapper;
using CrossCutting.Resources;
using Domain;
using Domain.Interfaces;
using Domain.Model;
using Microsoft.Extensions.Localization;

namespace Application.Services
{
    public class SpeciesService : ISpeciesService
    {
        private readonly IMapper _mapper;
        private readonly StarTrekContext _context;
        private readonly IStringLocalizer<Messages> _localizer;
        public SpeciesService(IMapper mapper, StarTrekContext context, IStringLocalizer<Messages> localizer)
        {
            _mapper = mapper;
            _context = context;
            _localizer = localizer;
        }

        public async Task<Species> GetById(short id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Species>> GetList(byte page, byte pageSize, Expression<Func<Species, bool>> predicate)
        {
            throw new NotImplementedException();
        }
    }
}
