using Domain.Model;
using Domain.ViewModel;

namespace Domain.Interfaces
{
    public interface ISeasonService : IService<Season, SeasonWithSerieIdVM, short>
    {
        Task<SeasonWithSerieIdVM> Create(CreateSeasonWithSerieIdDto dto);
        Task Update(short seasonId, UpdateSeasonDto dto);
    }
}
