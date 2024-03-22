using CrossCutting.AppModel;
using Domain.DTOs;
using Domain.ViewModel;

namespace Domain.Interfaces
{
    public interface ISerieService : IService<SerieVM, short>
    {
        Task<IEnumerable<SerieVM>> GetList(byte page, byte pageSize, string name);
        Task<SerieVM> Create(CreateSerieDto dto);
        Task Update(short id, UpdateSerieDto dto);
        Task<FileContent> Export();
    }
}
