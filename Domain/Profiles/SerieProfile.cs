using AutoMapper;
using CrossCutting.Enums;
using CrossCutting.Helpers;
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

            CreateMap<CreateSerieDto, Serie>()
                .ForMember(x => x.TimelineId, x => x.MapFrom(opt => opt.TimelineId.GetHashCode()))
                .ForMember(x => x.OriginalLanguageId,
                    x => x.MapFrom(opt => 
                        Enum.Parse<LanguageEnum>(RegexHelper.RemoveSpecialCharacters(opt.OriginalLanguageIso), true).GetHashCode()
                    )
                );
        }
    }
}
