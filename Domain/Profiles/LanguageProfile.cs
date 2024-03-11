using AutoMapper;
using Domain.Model;
using Domain.ViewModel;

namespace Domain.Profiles
{
    public class LanguageProfile : Profile
    {
        public LanguageProfile()
        {
            CreateMap<Language, LanguageVM>()
                .ForMember(x => x.Name, x => x.MapFrom(opt => opt.ResourceName));
        }
    }
}
