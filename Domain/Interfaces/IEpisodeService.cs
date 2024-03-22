using Domain.DTOs;
using Domain.Model;
using Domain.ViewModel;

namespace Domain.Interfaces
{
    public interface IEpisodeService : IService<EpisodeWithSeasonIdVM, int>
    {
        Task<IEnumerable<EpisodeWithSeasonIdVM>> GetList(byte page, byte pageSize);
        Task<EpisodeWithSeasonIdVM> Create(CreateEpisodeWithSeasonIdDto dto);
        Task Update(int episodeId, UpdateEpisodeDto dto);
    }
}
