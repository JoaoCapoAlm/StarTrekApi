using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Data.ViewModel;
using AutoMapper;
using Domain.Model;

namespace Domain.Profiles
{
    public class EpisodeProfile : Profile
    {
        public EpisodeProfile()
        {
            CreateMap<Episode, EpisodeVM>()
                .ForMember(x => x.ID, opt => opt.MapFrom(x => x.EpisodeId));
        }
    }
}
