using Application.Data.ViewModel;
using AutoMapper;
using Domain.Model;

namespace Domain.Profiles
{
    public class SeasonProfile : Profile
    {
        public SeasonProfile()
        {
            CreateMap<CreateSeasonWithSerieIdDto, CreateSeasonDto>();
            
            CreateMap<Season, SeasonVM>()
                .ForMember(x => x.ID, opt => opt.MapFrom(src => src.SeasonId));

            CreateMap<Season, SeasonWithSerieIdVM>()
                .ForMember(x => x.ID, opt => opt.MapFrom(x => x.SeasonId));
        }
    }
}
