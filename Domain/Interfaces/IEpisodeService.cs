using Domain.Model;
using Domain.ViewModel;

namespace Domain.Interfaces
{
    public interface IEpisodeService : IService<Episode, EpisodeWithSeasonIdVM, int>
    {
        Task<EpisodeWithSeasonIdVM> Create(CreateEpisodeWithSeasonIdDto dto);
        Task Update(int episodeId, UpdateEpisodeDto dto);
    }
}
