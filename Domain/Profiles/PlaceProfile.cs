using AutoMapper;
using Domain.DTOs;
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
                .ForMember(x => x.ID, x => x.MapFrom(opt => opt.PlaceId))
                .ForMember(x => x.Name, x => x.MapFrom(opt => opt.NameResource));

            CreateMap<Quadrant, QuadrantVM>()
                .ForMember(x => x.ID, x => x.MapFrom(opt => opt.QuadrantId))
                .ForMember(x => x.Name, x => x.MapFrom(opt => opt.QuadrantResource));

            CreateMap<CreatePlaceDto, Place>();
        }
    }
}
