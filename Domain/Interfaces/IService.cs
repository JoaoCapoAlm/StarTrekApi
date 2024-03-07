using System.Linq.Expressions;
using System.Numerics;

namespace Domain.Interfaces
{
    public interface IService<Model, Vm, Id>
        where Model : class
        where Vm : class
        where Id : INumber<Id>
    {
        Task<IEnumerable<Vm>> GetList(byte page, byte pageSize, Expression<Func<Model, bool>> predicate);
        Task<Vm> GetById(Id id);
    }
}
