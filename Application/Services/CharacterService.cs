using Domain.Interfaces;
using Domain.ViewModel;

namespace Application.Services
{
    public class CharacterService : ICharacterService
    {
        public Task<CharacterVM> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CharacterVM>> GetList(byte page, byte pageSize)
        {
            throw new NotImplementedException();
        }
    }
}
