using AutoMapper;
using Domain.Model;
using Domain.ViewModel;

namespace Domain.Profiles
{
    public class SerieProfile : Profile
    {
        public SerieProfile()
        {
            CreateMap<Serie, SerieVM>()
                .ForMember(x => x.ID, x => x.MapFrom(opt => opt.SerieId))
                .ForMember(x => x.Timeline, x => x.MapFrom(opt => opt.TimelineId))
                .ForMember(x => x.Synopsis, x => x.MapFrom(opt => opt.SynopsisResource))
                .ForMember(x => x.TranslatedName, x => x.MapFrom(opt => opt.TitleResource));
        }
    }
}
