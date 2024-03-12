using Domain.Model;
using Domain.ViewModel;

namespace Domain.Interfaces
{
    public interface ICharacterService : IService<Character, CharacterVM, int>
    {
    }
}
