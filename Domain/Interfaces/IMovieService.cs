using Domain.DTOs;
using Domain.ViewModel;

namespace Domain.Interfaces
{
    public interface IMovieService : IService<MovieVM, short>
    {
        Task<IEnumerable<MovieVM>> GetList(byte page, byte pageSize);
        Task<MovieVM> Create(CreateMovieDto dto);
        Task Update(short id, UpdateMovieDto dto);
    }
}
