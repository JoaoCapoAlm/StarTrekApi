using Domain.DTOs;
using Domain.Model;
using Domain.ViewModel;

namespace Domain.Interfaces
{
    public interface IPlaceService : IService<Place, PlaceVM, short>
    {
        Task<PlaceVM> Create(CreatePlaceDto dto);
    }
}
