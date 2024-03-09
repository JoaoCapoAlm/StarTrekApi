﻿using AutoMapper;
using Domain.Model;
using Domain.ViewModel;

namespace Domain.Profiles
{
    public class SeasonProfile : Profile
    {
        public SeasonProfile()
        {
            CreateMap<CreateSeasonWithSerieIdDto, CreateSeasonDto>();

            CreateMap<CreateSeasonWithSerieIdDto, Season>();

            CreateMap<Season, SeasonVM>()
                .ForMember(x => x.ID, opt => opt.MapFrom(x => x.SeasonId));

            CreateMap<Season, SeasonWithSerieIdVM>()
                .ForMember(x => x.ID, opt => opt.MapFrom(x => x.SeasonId));

            CreateMap<SeasonWithSerieIdVM, Season>()
                .ForMember(x => x.SeasonId, opt => opt.MapFrom(x => x.ID));

            CreateMap<CreateSeasonDto, Season>();
        }
    }
}
