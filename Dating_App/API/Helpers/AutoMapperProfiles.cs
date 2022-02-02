using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, MemberDto>()
            .ForMember(destination => destination.PhotoUrl,
             opt => { opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url); })
             .ForMember(destination => destination.Age,
             opt => { opt.MapFrom(calc => calc.DateOfBirth.CalculateAge()); });


            CreateMap<Photo, PhotoDto>();
        }
    }
}