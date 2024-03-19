using CrossCutting.Enums;
using Domain.DTOs;
using Domain.ViewModel;

namespace Domain.Interfaces
{
    public interface IPlaceService : IService<PlaceVM, short>
    {
        Task<IEnumerable<PlaceVM>> GetList(byte page, byte pageSize, QuadrantEnum? quadrant);
        Task<PlaceVM> Create(CreatePlaceDto dto);
    }
}
