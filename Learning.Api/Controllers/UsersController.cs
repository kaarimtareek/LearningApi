using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Data.Entities;
using DTOs.UserDTOs;
using Helpers.Mapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.CourseLibraryService;
using Services.LoggerService;
using Services.ResultObject;

namespace Learning.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILoggerService logger;
        private readonly IMapperHelper mapper;
        private readonly ICourseLibraryService courseLibraryService;

        public UsersController(ILoggerService logger ,IMapperHelper mapper,ICourseLibraryService courseLibraryService)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.courseLibraryService = courseLibraryService;
        }
       [HttpGet("{id}")]
       public async Task<IActionResult> GetUser(Guid id)
        {
           var result = await courseLibraryService.GetUserById(id);
            if(result.Success)
            {
                var successResult = result as SuccessOperationResult<User>;
                var user = mapper.MapTo<UserDto>(successResult.Result);
                var returnResult = new SuccessOperationResult<UserDto>
                {
                    Code = successResult.Code,
                    Result = user
                };
                return Ok(returnResult);
            }
            return Ok(result);

        }
        [HttpPost]
        public async Task<IActionResult> CreateUser ([FromBody] CreateUserDto createUserDto)
        {
            var user = mapper.MapTo<User>(createUserDto);
            user.PhoneNumber = FormatPhoneNumber(user.PhoneNumber);
            var result = await courseLibraryService.AddUser(user);
            if(result.Success)
            {
                var successResult = result as SuccessOperationResult<User>;
                var userDto = mapper.MapTo<UserDto>(successResult.Result);
                var returnResult = new SuccessOperationResult<UserDto>
                {
                    Code = successResult.Code,
                    Result = userDto
                };
                return Ok(returnResult);

            }
            return Ok(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser (Guid id)
        {
            //no need fo casting here , the return type is bool
            var result = await courseLibraryService.DeleteUser(id);
            return Ok(result);
        }
        [HttpPut()]
        public async Task<IActionResult> UpdateUser(UpdateUserDto updateUserDto)
        {
            var user = mapper.MapTo<User>(updateUserDto);
            var result = await courseLibraryService.UpdateUser(user);
            if (result.Success)
            {
                var successResult = result as SuccessOperationResult<User>;
                var userDto = mapper.MapTo<UserDto>(successResult.Result);
                var returnResult = new SuccessOperationResult<UserDto>
                {
                    Code = successResult.Code,
                    Result = userDto
                };
                return Ok(returnResult);
            }
            return Ok(result);
        }
        [HttpPut("changePassword")]
        public async Task<IActionResult> ChangeUserPassword([FromBody]ChangeUserPasswordDto userPasswordDto)
        {
            var result = await courseLibraryService.ChangeUserPassword(userPasswordDto);
            return Ok(result);
        }
        private string FormatPhoneNumber(string phone)
        {
            if (phone.StartsWith('0')) 
            phone = phone.TrimStart('0');
            return phone;
        }
    }
}