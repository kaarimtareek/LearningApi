using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Data.Entities;
using DTOs.UserDTOs;

namespace Learning.Api.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(u=>u.Country,o=>o.MapFrom(u=>u.CountryIso))
                .ReverseMap();
            CreateMap<CreateUserDto, User>().ForMember(u=>u.CountryIso,o=>o.MapFrom(u=>u.Country));
        }
    }
}
