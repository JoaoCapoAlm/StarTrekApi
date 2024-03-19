using Domain.ViewModel;

namespace Domain.Interfaces
{
    public interface ICrewService : IService<CrewVM, int>
    {
        Task<IEnumerable<CrewVM>> GetList(byte page, byte pageSize);
    }
}
