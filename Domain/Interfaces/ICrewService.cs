using Domain.Model;
using Domain.ViewModel;

namespace Domain.Interfaces
{
    public interface ICrewService : IService<Crew, CrewVM, int>
    {
    }
}
