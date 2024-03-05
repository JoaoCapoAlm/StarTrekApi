using Application.Data.ViewModel;

namespace Domain.Interfaces
{
    public interface ISerieService : IService<SerieVM, short>
    {
        Task<SerieVM> Create(CreateSerieDto dto);
        Task Update(short id, UpdateSerieDto dto);
    }
}
