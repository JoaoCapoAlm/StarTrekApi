using Application.Data.ViewModel;

namespace Domain.Interfaces
{
    public interface ISeasonService : IService<SeasonWithSerieIdVM, short>
    {
        Task<SeasonWithSerieIdVM> Create(CreateSeasonWithSerieIdDto dto);
        Task Update(short seasonId, UpdateSeasonDto dto);
    }
}
