using System.Numerics;

namespace Domain.Interfaces
{
    public interface IService<Vm, Id>
        where Vm : class
        where Id : INumber<Id>
    {
        Task<Vm> GetById(Id id);
    }
}
