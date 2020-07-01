using AutoMapper;
using DTOs.AuthorDTOs;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTOs.Profiles
{
    //why profile doesnt work in  Data class library??
     public  class AuthorProfile :Profile
    {
        public AuthorProfile()
        {
            CreateMap<Author, AuthorDto>()
                .ForMember(m => m.Name,
                options => options.MapFrom(author => $"{author.FirstName} {author.LastName}"))
                .ForMember(m => m.Age,
                options => options.MapFrom(author => FromDateToAge.ConvertDate(author.DateOfBirth))).ReverseMap();
            CreateMap<CreateAuthorDto, Author>().ReverseMap();

        }
    }
     static class FromDateToAge
    {
        public static int ConvertDate(DateTimeOffset date)
        {
            var now = DateTime.Now;
            int age = now.Year - date.Year;
            return age;
        }
    }
}
