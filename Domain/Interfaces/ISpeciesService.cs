using Domain.DTOs;
using Domain.Model;
using Domain.ViewModel;

namespace Domain.Interfaces
{
    public interface ISpeciesService : IService<Species, SpeciesVM, short>
    {
        Task<SpeciesVM> CreateSpecies(CreateSpeciesDto dto);
    }
}
