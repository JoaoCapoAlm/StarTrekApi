using AutoMapper;
using Domain.DTOs;
using Domain.Model;
using Domain.ViewModel;

namespace Domain.Profiles
{
    public class SpeciesProfile : Profile
    {
        public SpeciesProfile()
        {
            CreateMap<CreateSpeciesDto, Species>();

            CreateMap<Species, SpeciesVM>()
                .ForMember(x => x.ID, x => x.MapFrom(opt => opt.SpeciesId))
                .ForMember(x => x.Name, x => x.MapFrom(opt => opt.ResourceName));
        }
    }
}
