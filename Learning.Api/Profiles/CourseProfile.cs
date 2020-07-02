
using AutoMapper;
using DTOs.CourseDTOs;
using Entities;

namespace Learning.Api.Profiles
{
    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            CreateMap<CourseDto, Course>().ReverseMap();
            CreateMap<AddCourseDto, Course>().ReverseMap();
            CreateMap<UpdateCourseDto, Course>().ReverseMap();
        }
    }
}
