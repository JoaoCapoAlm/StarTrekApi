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
                .ForMember(x => x.ID, x => x.MapFrom(opt => opt.SerieId));
        }
    }
}
