using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DTOs.AuthorDTOs;
using DTOs.QueryParamters;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NLog;
using Services.CourseLibraryService;
using Services.LoggerService;
using Services.PaginationService;
using Services.ResultObject;

namespace Learning.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly ICourseLibraryService courseLibraryService;
        private readonly IMapper mapper;
        private readonly ILoggerService loggerService;

        public AuthorsController(ICourseLibraryService courseLibraryService,IMapper mapper,ILoggerService loggerService)
        {
            this.courseLibraryService = courseLibraryService;
            this.mapper = mapper;
            this.loggerService = loggerService;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorDto>> GetAuthor(Guid id,[FromQuery] bool withCourses = false )
        {
            var result =await courseLibraryService.GetAuthor(id,withCourses);
            if(result.Success)
            {
                var successOperation = result as SuccessOperationResult<Author>;
                var authorDto = mapper.Map<AuthorDto>(successOperation.Result);
                var operationReturn = new SuccessOperationResult<AuthorDto>
                {
                    Result = authorDto,
                    Code = successOperation.Code,
                };
                return Ok(operationReturn);
            }
            
            return Ok(result);

        }
        [HttpGet()]
        public  ActionResult<IEnumerable<AuthorDto>> GetAuthors([FromQuery] AuthorsQueryParamters queryParamters)
        {
            try
            {
                OperationResult<PaginationList<Author>> result =  courseLibraryService.GetAuthors(queryParamters);
                if(result.Success)
                {
                    var successOperation = result as SuccessOperationResult<PaginationList<Author>>;
                    IEnumerable<AuthorDto> mappingResult = mapper.Map<IEnumerable<AuthorDto>>(successOperation.Result.ListData);
                    var paginationMetaData = new
                    {
                        totalCount = successOperation.Result.TotalCount,
                        totalPages = successOperation.Result.TotalPages,
                        currentPage = successOperation.Result.CurrentPage,
                        pageSize = successOperation.Result.PageSize
                    };
                    Response.Headers.Add("paginationMetaData", JsonConvert.SerializeObject(paginationMetaData));
                    var operationReturn = new SuccessOperationResult<IEnumerable<AuthorDto>>
                    {
                        Result = mappingResult,
                        Code = successOperation.Code,

                    };
                    return Ok(operationReturn);
                }else
                {
                    var failedOperation = result as FailedOperationResult<PaginationList<Author>>;
                    return Ok(failedOperation);
                }
                
            }
            catch (Exception ex)
            {
                loggerService.Error($"Error in Authors Controller GetAuthors {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }
    }
}