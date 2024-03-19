using Domain.DTOs;
using Domain.ViewModel;

namespace Domain.Interfaces
{
    public interface ISeasonService : IService<SeasonWithSerieIdVM, short>
    {
        Task<IEnumerable<SeasonWithSerieIdVM>> GetList(byte page, byte pageSize);
        Task<SeasonWithSerieIdVM> Create(CreateSeasonWithSerieIdDto dto);
        Task Update(short seasonId, UpdateSeasonDto dto);
    }
}
