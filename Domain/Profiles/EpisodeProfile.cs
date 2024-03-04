using Application.Data.ViewModel;
using AutoMapper;
using Domain.Model;

namespace Domain.Profiles
{
    public class EpisodeProfile : Profile
    {
        public EpisodeProfile()
        {
            CreateMap<Episode, EpisodeVM>()
                .ForMember(x => x.ID, opt => opt.MapFrom(x => x.EpisodeId));

            CreateMap<Episode, EpisodeWithSeasonIdVM>()
                .ForMember(x => x.ID, opt => opt.MapFrom(x => x.EpisodeId))
                .ForMember(x => x.TranslatedTitle, x => x.MapFrom(y => y.TitleResource))
                .ForMember(x => x.TranslatedSynopsis, x => x.MapFrom(y => y.SynopsisResource));

            CreateMap<CreateEpisodeWithSeasonIdDto, Episode>();
            CreateMap<CreateEpisodeWithSeasonIdDto, CreateEpisodeDto>();
        }
    }
}
