using Domain.ViewModel;

namespace Domain.Interfaces
{
    public interface ICharacterService : IService<CharacterVM, int>
    {
        Task<IEnumerable<CharacterVM>> GetList(byte page, byte pageSize);
    }
}
