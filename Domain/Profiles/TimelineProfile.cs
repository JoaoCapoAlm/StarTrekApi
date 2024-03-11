using AutoMapper;
using Domain.Model;
using Domain.ViewModel;

namespace Domain.Profiles
{
    public class TimelineProfile : Profile
    {
        public TimelineProfile()
        {
            CreateMap<Timeline, TimelineVM>()
                .ForMember(x => x.ID, x => x.MapFrom(opt => opt.TimelineId));
        }
    }
}
