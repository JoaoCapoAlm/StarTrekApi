using Domain.Model;
using Domain.ViewModel;

namespace Domain.Interfaces
{
    public interface IMovieService : IService<Movie, MovieVM, short>
    {
        Task<MovieVM> Create(CreateMovieDto dto);

        Task Update(short id, UpdateMovieDto dto);
    }
}
