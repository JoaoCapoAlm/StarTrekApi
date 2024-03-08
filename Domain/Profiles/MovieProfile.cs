using AutoMapper;
using Domain.Model;
using Domain.ViewModel;

namespace Domain.Profiles
{
    public class MovieProfile : Profile
    {
        public MovieProfile()
        {
            CreateMap<Movie, MovieVM>()
                .ForMember(x => x.Id, x => x.MapFrom(opt => opt.MovieId))
                .ForMember(x => x.Synopsis, x => x.MapFrom(opt => opt.SynopsisResource))
                .ForMember(x => x.TranslatedName, x => x.MapFrom(opt => opt.TitleResource))
                .ForMember(x => x.OriginalLanguage, x => x.MapFrom(opt => opt.Languages.CodeISO))
                .ForMember(x => x.Timeline, x => x.MapFrom(opt => opt.Timeline));
        }
    }
}
