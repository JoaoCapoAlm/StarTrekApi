using AutoMapper;
using Domain.Model;
using Domain.ViewModel;

namespace Domain.Profiles
{
    public class PlaceProfile : Profile
    {
        public PlaceProfile()
        {
            CreateMap<PlaceType, PlaceTypeVM>()
                .ForMember(x => x.ID, x => x.MapFrom(opt => opt.PlaceTypeId));

            CreateMap<Place, PlaceVM>()
                .ForMember(x => x.ID, x => x.MapFrom(opt => opt.PlaceId));
        }
    }
}
