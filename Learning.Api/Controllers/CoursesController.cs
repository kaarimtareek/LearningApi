using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DTOs.CourseDTOs;
using DTOs.QueryParamters;
using Entities;
using Helpers.Mapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.CourseLibraryService;
using Services.LoggerService;
using Services.PaginationService;
using Services.ResultObject;

namespace Learning.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseLibraryService _courseLibraryService;
        private readonly ILoggerService _logger;
        private readonly IMapperHelper _mapper;
        public CoursesController(ICourseLibraryService courseLibraryService, IMapperHelper mapper, ILoggerService logger)
        {
            _courseLibraryService = courseLibraryService;
            _mapper = mapper;
            _logger = logger;
        }
        [HttpGet()]
        public async Task<ActionResult<OperationResult<IEnumerable<CourseDto>>>> GetCourses([FromQuery]CoursesQueryParameters queryParameters)
        {
            OperationResult<PaginationList<Course>> result = await _courseLibraryService.GetCourses(queryParameters);
            if(result.Success)
            {
                var successOperation = result as SuccessOperationResult<PaginationList<Course>>;
                var coursesDto = _mapper.MapTo<IEnumerable<CourseDto>>(successOperation.Result.ListData);
                var operationReturn = new SuccessOperationResult<IEnumerable<CourseDto>>
                {
                    Result = coursesDto,
                    Code = successOperation.Code,

                };
                return Ok(operationReturn);
            }
            var failedOperation = result as FailedOperationResult<PaginationList<Course>>;
            return Ok(failedOperation);


        }
        [HttpGet("/{courseId}")]
        public async Task<ActionResult<CourseDto>> GetCourse(Guid courseId)
        {
            var result = await _courseLibraryService.GetCourseById(courseId);
            if(result.Success)
            {
                var successOperation = result as SuccessOperationResult<Course>;
                var courseDto = _mapper.MapTo<CourseDto>(successOperation.Result);
                var operationResult = new SuccessOperationResult<CourseDto>
                {
                    Result = courseDto,
                    Code = successOperation.Code,

                };

                return Ok(operationResult);
            }
            var failedOperation = result as FailedOperationResult<Course>;
            return Ok(failedOperation);


        }

        [HttpPost()]
        public async Task<IActionResult> AddCourse([FromBody] AddCourseDto addCourseDto)
        {
            var course = _mapper.MapTo<Course>(addCourseDto);
            var result = await _courseLibraryService.AddCourse(course);
            if(result.Success)
            {
                var successOperation = result as SuccessOperationResult<Course>;
                var courseDto = _mapper.MapTo<CourseDto>(successOperation.Result);
                var operationReturn = new SuccessOperationResult<CourseDto>
                {
                    Result = courseDto,
                    Code = successOperation.Code,

                };
                return Ok(operationReturn);
            }
            var failedOperation = result as FailedOperationResult<Course>;
            return Ok(failedOperation);


        }

        [HttpDelete("{courseId}")]
        public async Task<IActionResult> DeleteCourse(Guid courseId)
        {
            var result = await _courseLibraryService.DeleteCourse(courseId);
            if(result.Success)
            {
                var successOperation = result as SuccessOperationResult<bool>;
                return Ok(successOperation);
            }
            var failedOperation = result as FailedOperationResult<bool>;
            return Ok(failedOperation);


        }
        [HttpPut]
        public async Task<IActionResult>UpdateCourse(UpdateCourseDto updateCourseDto)
        {
            var course = _mapper.MapTo<Course>(updateCourseDto);
            var result = await _courseLibraryService.UpdateCourse(course);
            if(result.Success)
            {
                var successOperation = result as SuccessOperationResult<Course>;
                var courseDto = _mapper.MapTo<CourseDto>(successOperation.Result);
                return Ok(new SuccessOperationResult<CourseDto>
                {
                    Code = successOperation.Code,
                    Result = courseDto,
                });
            }
            var failedOperation = result as FailedOperationResult<Course>;
            return Ok(failedOperation);
        }
    }
}
