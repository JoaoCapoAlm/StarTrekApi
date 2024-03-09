using AutoMapper;
using Domain.Model;
using Domain.ViewModel;

namespace Domain.Profiles
{
    public class CrewProfile : Profile
    {
        public CrewProfile()
        {
            CreateMap<Crew, CrewVM>()
                .ForMember(x => x.Id, x => x.MapFrom(opt => opt.CrewId))
                .ForMember(x => x.Country, x => x.MapFrom(opt => opt.Country.ResourceName));
        }
    }
}
