using Data.DbContexts;
using DTOs.AuthorDTOs;
using DTOs.CourseDTOs;
using DTOs.QueryParamters;
using Entities;
using Microsoft.EntityFrameworkCore;
using Services.CompiledQueries;
using Services.FilterationService;
using Services.LoggerService;
using Services.OperationCodes;
using Services.PaginationService;
using Services.ResultObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Services.CourseLibraryService
{
    public class CourseLibraryService : ICourseLibraryService
    {
        private readonly DbContextOptions<CourseLibraryContext> options;
        private readonly IFilterationService filterationService;
        private readonly ILoggerService logger;


        public CourseLibraryService(DbContextOptions<CourseLibraryContext> options, IFilterationService filterationService, ILoggerService loggerService)
        {

            this.options = options;
            this.filterationService = filterationService;
            this.logger = loggerService;
        }

        public async Task<OperationResult<Author>> GetAuthor(Guid id,bool withCourses =false)
        {
            using (CourseLibraryContext context = new CourseLibraryContext(options))
            {
                try
                {
                    Author author;
                    if (withCourses)
                    {
                        author = await AuthorCompiledQueries.GetAuthorWithCourses(context, id);
                    }
                    else
                    {
                        author = await AuthorCompiledQueries.GetAuthor(context, id);
                    }
                    if(author != null)
                    {
                        return new SuccessOperationResult<Author>
                        {
                            Code = ConstOperationCodes.AUTHOR_FOUND,
                            Result = author
                        };
                    }
                    else
                    {
                        return new FailedOperationResult<Author>
                        {
                            Code = ConstOperationCodes.AUTHOR_NOT_FOUND
                        };
                    }
                    
                }
                catch(Exception e)
                {
                    logger.Error($"error in course library context getting author {e.Message}");
                    return new FailedOperationResult<Author>
                    {
                        Code = ConstOperationCodes.FAILED_OPERATION  
                    };
                }
               
            }
        }


        public OperationResult<IEnumerable<Author>> GetAuthors()
        {
            using (CourseLibraryContext context = new CourseLibraryContext(options))
            {
                try
                {
                    var authors = AuthorCompiledQueries.GetAuthors.Invoke(context);
                    return new SuccessOperationResult<IEnumerable<Author>>
                    {
                        Code = ConstOperationCodes.SUCCESS_OPERATION,
                        Result = authors
                    };
                }catch(Exception e)
                {
                    logger.Error("error in get authors :" + e.Message);
                    return new FailedOperationResult<IEnumerable<Author>>
                    {
                        
                        Code = ConstOperationCodes.FAILED_OPERATION
                    };
                }
                
            }

        }

        public OperationResult<PaginationList<Author>> GetAuthors(AuthorsQueryParamters queryParamters)
        {
            bool hasOrderBy = queryParamters.OrderBy != null;
            bool hasSearchQuery = queryParamters.SearchQuery != null;
            try
            {
                using (CourseLibraryContext context = new CourseLibraryContext(options))
                {
                    var collection = context.Authors as IQueryable<Author>;
                    if (hasSearchQuery)
                    {
                        collection = collection.Where(a => a.FirstName.Contains(queryParamters.SearchQuery) ||
                                         a.LastName.Contains(queryParamters.SearchQuery) ||
                                         a.MainCategory.Contains(queryParamters.SearchQuery));
                       
                    }
                    if (hasOrderBy)
                    {
                        string orderstring = filterationService.GetFiltrationString<AuthorDto, Author>(queryParamters.OrderBy);

                        collection = collection.OrderBy(orderstring);
                    }
                    logger.Info($"Request authors with query paramters : {queryParamters}");
                    //TODO add pagination here
                    PaginationList<Author> result =  PaginationList<Author>.CreatePagination(collection, queryParamters.PageSize, queryParamters.PageNumber);
                    return new SuccessOperationResult<PaginationList<Author>>
                    { 
                        Result = result,
                        Code = ConstOperationCodes.SUCCESS_OPERATION
                    };
                }
            }
            catch (Exception e)
            {
                logger.Error($"Error in Course library Service GetAuthors : {e}");
                return new FailedOperationResult<PaginationList<Author>>
                {
                    Code = ConstOperationCodes.FAILED_OPERATION
                };
            }

        }

        public async Task<OperationResult<PaginationList<Course>>> GetCourses(CoursesQueryParameters queryParameters)
        {
            bool hasOrderBy = queryParameters.OrderBy != null;
            bool hasSearchQuery = queryParameters.SearchQuery != null;
            using (CourseLibraryContext context = new CourseLibraryContext(options))
            {
                try
                {
                    var collection = context.Courses as IQueryable<Course>;
                    if (hasSearchQuery)
                    {
                        collection = context.Courses.Where(c => c.Title.Contains(queryParameters.SearchQuery));
                    }
                    if (hasOrderBy)
                    {
                        string orderString = filterationService.GetFiltrationString<CourseDto, Course>(queryParameters.OrderBy);

                        collection = context.Courses.OrderBy(orderString);
                    }
                    var courses = PaginationList<Course>.CreatePagination(collection, queryParameters.PageSize, queryParameters.PageNumber);
                    return new SuccessOperationResult<PaginationList<Course>>
                    {
                        Result = courses,
                        Code = ConstOperationCodes.SUCCESS_OPERATION
                    };
                }
                catch(Exception e)
                {
                    logger.Error($"error in courses library service get courses : {e.Message}");
                    return new FailedOperationResult<PaginationList<Course>>
                    {
                        Code = ConstOperationCodes.FAILED_OPERATION
                    };
                }
                
            }
        }
        public async Task<OperationResult<IEnumerable<Course>>> GetCoursesForAuthor(Guid authorId)
        {
            using (CourseLibraryContext context = new CourseLibraryContext(options))
            {
                try
                {
                   var author = await AuthorCompiledQueries.GetAuthor(context, authorId);
                    if(author ==null)
                    {
                        return new FailedOperationResult<IEnumerable<Course>>
                        {
                            Code = ConstOperationCodes.AUTHOR_NOT_FOUND,
                        };
                    }
                    var result = await context.Courses.Where(c => c.AuthorId == authorId).ToListAsync();
                    return new SuccessOperationResult<IEnumerable<Course>> 
                    { 
                        Result = result,
                        Code = ConstOperationCodes.SUCCESS_OPERATION
                    };
                }
              catch(Exception e)
                {
                    logger.Error($"error in course library get courses for author {e.Message}");
                    return new FailedOperationResult<IEnumerable<Course>>
                    {
                        Code = ConstOperationCodes.FAILED_OPERATION
                    };
                }
            }
        }

        public OperationResult<PaginationList<Course>> GetCourses(CoursesQueryParameters queryParameters, Guid authorId) => throw new NotImplementedException();
        public async Task<OperationResult<Course>> GetCourseById(Guid courseId)
        {
            using (CourseLibraryContext context = new CourseLibraryContext(options))
            {
                try
                {
                    var result = await CoursesCompiledQueries.GetCourseById(context, courseId);
                   if(result ==null)
                    {
                        return new FailedOperationResult<Course>
                        {
                            Code = ConstOperationCodes.COURSE_NOT_FOUND
                        };
                    }
                    return new SuccessOperationResult<Course>
                    {
                        Result = result
                    };
                }
                catch (Exception e)
                {
                    logger.Error($"error in course library context get course by id {e.Message} ");
                    return new FailedOperationResult<Course>
                    {
                        Code = ConstOperationCodes.FAILED_OPERATION,
                    };
                }

            }
        }
      

        public async Task<OperationResult<Course>> AddCourse(Course course)
        {
            using (CourseLibraryContext context = new CourseLibraryContext(options))
            {
                try
                {
                   bool isCourseExistWithSameName  = await context.Courses.AnyAsync(c => c.Title == course.Title);
                    if(isCourseExistWithSameName)
                    {
                        return new FailedOperationResult<Course>
                        {
                            Code = ConstOperationCodes.COURSE_NAME_ALREADY_EXISTS
                        };
                    }
                    await context.AddAsync(course);
                    await context.SaveChangesAsync();
                    return new SuccessOperationResult<Course>
                    {
                       Code = ConstOperationCodes.COURSE_CREATED,
                        Result = course
                    };
                }
          
                catch(Exception e)
                {
                    logger.Error("error in adding course " + e.Message);
                    return new FailedOperationResult<Course>
                    {
                       Code = ConstOperationCodes.FAILED_OPERATION
                    };
                }
            }
            
        }

        public async Task<bool> IsCourseExist(Guid courseId)
        {
            using (CourseLibraryContext context = new CourseLibraryContext(options))
            {
                bool isCourseExist = await CoursesCompiledQueries.IsCourseExist(context, courseId);
                return isCourseExist;
            }
        }
        public async Task<OperationResult<bool>> DeleteCourse(Guid courseId)
        {
            using (CourseLibraryContext context = new CourseLibraryContext(options))
            {
                try
                {
                    if( ! await IsCourseExist(courseId))
                    {
                        return new FailedOperationResult<bool>
                        {
                            Code = ConstOperationCodes.COURSE_NOT_FOUND,
                        };
                    }
                    var course = await context.Courses.FindAsync(courseId);
                    context.Courses.Remove(course);
                    await context.SaveChangesAsync();
                    return new SuccessOperationResult<bool>
                    {
                        Code = ConstOperationCodes.COURSE_DELETED,
                        Result = true,
                    };
                }
                catch(Exception e)
                {
                    logger.Error($"error in delete course {e} {e.Message}");
                    return new FailedOperationResult<bool>
                    {
                       Code =ConstOperationCodes.FAILED_OPERATION
                    };
                }
               
            }
        }
   
        
    }
}
