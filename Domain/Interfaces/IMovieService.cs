using Domain.ViewModel;

namespace Domain.Interfaces
{
    public interface IMovieService : IService<MovieVM, short>
    {
        Task<MovieVM> Create(CreateMovieDto dto);

        Task Update(short id, UpdateMovieDto dto);
    }
}
