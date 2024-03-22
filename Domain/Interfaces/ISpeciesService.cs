using Domain.DTOs;
using Domain.Model;
using Domain.ViewModel;

namespace Domain.Interfaces
{
    public interface ISpeciesService : IService<SpeciesVM, short>
    {
        Task<IEnumerable<SpeciesVM>> GetList(byte page, byte pageSize);
        Task<SpeciesVM> CreateSpecies(CreateSpeciesDto dto);
    }
}
