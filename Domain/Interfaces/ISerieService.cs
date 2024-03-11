using CrossCutting.AppModel;
using Domain.DTOs;
using Domain.Model;
using Domain.ViewModel;

namespace Domain.Interfaces
{
    public interface ISerieService : IService<Serie, SerieVM, short>
    {
        Task<SerieVM> Create(CreateSerieDto dto);
        Task Update(short id, UpdateSerieDto dto);
        Task<FileContent> Export();
    }
}
