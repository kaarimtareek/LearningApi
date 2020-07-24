using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Data.Entities;
using DTOs.UserDTOs;
using Helpers.Mapper;
using Moq;
using Xunit;

namespace XLearningApiUnitTest.TestMappers
{
    public class UserMapperTest
    {
       
        public Mock<User> mockUser;
        public Mock<UserDto> mockUserDto;
        public Mock<CreateUserDto> mockCreateUserDto;
        public MapperHelper mockHelper;
        public MapperConfiguration mapperConfiguration;
        public UserMapperTest()
        {
            mockCreateUserDto = new Mock<CreateUserDto>();
            mockUser = new Mock<User>();
            mockUserDto = new Mock<UserDto>();
            mapperConfiguration = new MapperConfiguration(config =>
            config.CreateMap<CreateUserDto, User>(MemberList.Source)
            .ForMember(u => u.Country, opt => opt.Ignore())
            .ForMember(u => u.CountryIso, o => o.MapFrom(u => $"{u.Country}")));
            
           var mapper = mapperConfiguration.CreateMapper();
            mockHelper = new MapperHelper(mapper) ;


        }
        [Fact]
        public void ShouldMapUserToUserDto ()
        {
            //act 
            mapperConfiguration.AssertConfigurationIsValid();
            var result = mockHelper.MapTo<User>(mockCreateUserDto.Object);
            Assert.Equal(result, mockUser.Object);

            
        } 
    }
}
