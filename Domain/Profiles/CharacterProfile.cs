using AutoMapper;
using Domain.DTOs;
using Domain.Model;
using Domain.ViewModel;

namespace Domain.Profiles
{
    public class CharacterProfile : Profile
    {
        public CharacterProfile()
        {
            CreateMap<CreateCharacterDto, Character>();
            CreateMap<Character, CharacterVM>()
                .ForMember(x => x.ID, x => x.MapFrom(y => y.CharacterId));
        }
    }
}
