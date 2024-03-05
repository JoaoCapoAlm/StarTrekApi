namespace Domain.Interfaces
{
    public interface IService<Vm, Id>
    {
        Task<IEnumerable<Vm>> GetList(byte page, byte pageSize);
        Task<Vm> GetById(Id id);
    }
}
