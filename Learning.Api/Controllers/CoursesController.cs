using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DTOs.CourseDTOs;
using DTOs.QueryParamters;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Services.CourseLibraryService;
using Services.LoggerService;
using Services.PaginationService;
using Services.ResultObject;

namespace Learning.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseLibraryService _courseLibraryService;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;
        public CoursesController(ICourseLibraryService courseLibraryService, IMapper mapper, ILoggerService logger)
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
                var coursesDto = _mapper.Map<IEnumerable<CourseDto>>(successOperation.Result.ListData);
                var operationReturn = new SuccessOperationResult<IEnumerable<CourseDto>>
                {
                    Result = coursesDto
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
                var courseDto = _mapper.Map<CourseDto>(successOperation.Result);
                var operationResult = new SuccessOperationResult<CourseDto>
                {
                    Result = courseDto
                };

                return Ok(operationResult);
            }
            var failedOperation = result as FailedOperationResult<Course>;
            return Ok(failedOperation);


        }

        [HttpPost()]
        public async Task<IActionResult> AddCourse([FromBody] AddCourseDto addCourseDto)
        {
            var course = _mapper.Map<Course>(addCourseDto);
            var result = await _courseLibraryService.AddCourse(course);
            if(result.Success)
            {
                var successOperation = result as SuccessOperationResult<Course>;
                var courseDto = _mapper.Map<CourseDto>(successOperation.Result);
                var operationReturn = new SuccessOperationResult<CourseDto>
                {
                    Result = courseDto
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
    }
}
