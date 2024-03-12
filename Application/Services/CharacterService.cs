using System.Linq.Expressions;
using Domain.Interfaces;
using Domain.Model;
using Domain.ViewModel;

namespace Application.Services
{
    public class CharacterService : ICharacterService
    {
        public Task<CharacterVM> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CharacterVM>> GetList(byte page, byte pageSize, Expression<Func<Character, bool>> predicate)
        {
            throw new NotImplementedException();
        }
    }
}
