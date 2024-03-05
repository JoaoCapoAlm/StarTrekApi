using Application.Data.ViewModel;

namespace Domain.Interfaces
{
    public interface IEpisodeService : IService<EpisodeWithSeasonIdVM, int>
    {
        Task<EpisodeWithSeasonIdVM> Create(CreateEpisodeWithSeasonIdDto dto);
        Task Update(int episodeId, UpdateEpisodeDto dto);
    }
}
