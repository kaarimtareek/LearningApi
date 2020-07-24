using Data.DbContexts;
using Data.Entities;
using DTOs.AuthorDTOs;
using DTOs.CourseDTOs;
using DTOs.QueryParamters;
using Entities;
using Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        public async Task<OperationResult<Author>> AddAuthor(Author author)
        {
            using(CourseLibraryContext context = new CourseLibraryContext(options))
            {
                try
                {
                    bool isAuthorExisted = await AuthorCompiledQueries.IsExistSameAuthorName.Invoke(context, author.FirstName, author.LastName);
                    if (isAuthorExisted)
                    {
                        return new FailedOperationResult<Author>
                        {
                            Code = ConstOperationCodes.AUTHOR_NAME_ALREADY_EXISTS,
                        };
                    }
                    await context.Authors.AddAsync(author);
                    await context.SaveChangesAsync();
                    return new SuccessOperationResult<Author>
                    {
                        Code = ConstOperationCodes.AUTHOR_CREATED,
                        Result = author
                    };
                }
                catch(Exception e)
                {
                    logger.Error($"error in adding author {e.Message}");
                    return new FailedOperationResult<Author> 
                    { 
                        Code = ConstOperationCodes.FAILED_OPERATION
                    };
                }
            }
        }

        public async Task<OperationResult<Author>> UpdateAuthor(Author author)
        {
          using (CourseLibraryContext context = new CourseLibraryContext (options))
            {
                try
                {
                    var isAuthorExist = await AuthorCompiledQueries.IsAuthorExist.Invoke(context,author.Id);
                    if (!isAuthorExist)
                    {
                        return new FailedOperationResult<Author>
                        {
                            Code = ConstOperationCodes.AUTHOR_NOT_FOUND,
                        };
                    }
                    
                    context.Authors.Update(author);
                    await context.SaveChangesAsync();
                    return new SuccessOperationResult<Author>
                    {
                        Code = ConstOperationCodes.AUTHOR_UPDATED,
                        Result = author,
                    };
                }
                catch(Exception e)
                {
                    logger.Error($"error in update author {e.Message}");
                    return new FailedOperationResult<Author>
                    {
                        Code = ConstOperationCodes.FAILED_OPERATION
                    };
                }
              
            }
        }
        public async Task<OperationResult<Course>> UpdateCourse(Course course)
        {
            using (CourseLibraryContext context = new CourseLibraryContext(options))
            {
                try
                {
                    var author = await AuthorCompiledQueries.GetAuthor.Invoke(context, course.AuthorId);
                    if (author == null)
                    {
                        return new FailedOperationResult<Course>
                        {
                            Code = ConstOperationCodes.AUTHOR_NOT_FOUND,
                        };
                    }
                    var isCourseExist = await CoursesCompiledQueries.IsCourseExistForAuthor.Invoke(context, course.Id,course.AuthorId);
                    if (!isCourseExist)
                    {
                        return new FailedOperationResult<Course>
                        {
                            Code = ConstOperationCodes.COURSE_NOT_FOUND
                        };
                    }
                    context.Courses.Update(course);
                    await context.SaveChangesAsync();
                    return new SuccessOperationResult<Course>
                    {
                        Code = ConstOperationCodes.COURSE_UPDATED,
                        Result = course,
                    };
                }
              catch(Exception e)
                {
                    logger.Error($"error in updating course {e.Message}");
                    return new FailedOperationResult<Course>
                    {
                        Code = ConstOperationCodes.FAILED_OPERATION,
                    };
                }
            }
        }

        public async Task<OperationResult<User>> AddUser(User user)
        {
            using (CourseLibraryContext context = new CourseLibraryContext(options))
            {
                try
                {
                    bool isPhoneExists = await UserCompiledQueries.IsPhoneNumberExists.Invoke(context, user.PhoneNumber);
                    if (isPhoneExists)
                    {
                        return new FailedOperationResult<User>
                        {
                            Code = ConstOperationCodes.USER_PHONE_ALREADY_EXISTS
                        };
                    }
                    bool isEmailExists = await UserCompiledQueries.IsEmailExists.Invoke(context, user.Email);
                    if (isEmailExists)
                    {
                        return new FailedOperationResult<User>
                        {
                            Code = ConstOperationCodes.USER_EMAIL_ALREADY_EXISTS
                        };
                    }
                    var country = await context.Countries.FirstOrDefaultAsync(c => c.Iso == user.CountryIso);
                    if (country == null)
                    {
                        return new FailedOperationResult<User>
                        {
                            Code = ConstOperationCodes.INVALID_COUNTRY
                        };
                    }
                    user.Country = country;
                    string randomSalt = PasswordHasher.GenerateRandomSalt();
                  string hashedPassword =  PasswordHasher.GenerateHash(user.Password,randomSalt);
                    user.Salt = randomSalt;
                    user.Password = hashedPassword;
                    await context.Users.AddAsync(user);
                    await context.SaveChangesAsync();
                    return new SuccessOperationResult<User>
                    {
                        Code = ConstOperationCodes.USER_CREATED,
                        Result = user
                    };
                }
                catch(Exception e)
                {
                    logger.Error("Error adding user : " + e.Message);
                    return new FailedOperationResult<User>
                    {
                        Code = ConstOperationCodes.FAILED_OPERATION,
                        Message = e.Message
                    };
                }
              
            }
        }
        public async Task<OperationResult<User>> GetUserById(Guid id)
        {
            using (CourseLibraryContext context = new CourseLibraryContext(options))
            {
                try
                {
                    var user = await UserCompiledQueries.GetUserById.Invoke(context, id);
                    if (user == null)
                    {
                        return new FailedOperationResult<User>
                        {
                            Code = ConstOperationCodes.USER_NOT_FOUND
                        };
                    }
                    return new SuccessOperationResult<User>
                    {
                        Code = ConstOperationCodes.USER_FOUND,
                        Result = user,
                    };
                }
                catch (Exception e)
                {
                    logger.Error($"Error getting user with id {id} : {e.Message}");
                    return new FailedOperationResult<User>
                    {
                        Code = ConstOperationCodes.FAILED_OPERATION,
                    };
                }
            }
        }
        public async Task<OperationResult<bool>> DeleteUser(Guid Id)
        {
            using (CourseLibraryContext context = new CourseLibraryContext(options))
            {
                try
                {
                    var user = await UserCompiledQueries.GetUserById.Invoke(context, Id);
                    if (user == null)
                    {
                        return new FailedOperationResult<bool>
                        {
                            Code = ConstOperationCodes.USER_NOT_FOUND
                        };
                    }
                    context.Users.Remove(user);
                    await context.SaveChangesAsync();
                    return new SuccessOperationResult<bool>
                    {
                        Code = ConstOperationCodes.USER_DELETED,
                        Result = true,
                    };
                }
                catch (Exception e)
                {
                    logger.Error($"Error getting user with id {Id} : {e.Message}");
                    return new FailedOperationResult<bool>
                    {
                        Code = ConstOperationCodes.FAILED_OPERATION,
                    };
                }
            }
        }
        public async Task<OperationResult<User>> UpdateUser(User updatedUser)
        {
            using (CourseLibraryContext context = new CourseLibraryContext(options))
            {
                try
                {
                    User user = await UserCompiledQueries.GetUserById.Invoke(context, updatedUser.Id);
                    if (user == null)
                    {
                        return new FailedOperationResult<User>
                        {
                            Code = ConstOperationCodes.USER_NOT_FOUND
                        };
                    }
                    var country = await context.Countries.FirstOrDefaultAsync(c => c.Iso == updatedUser.CountryIso);
                    if(country ==null)
                    {
                        return new FailedOperationResult<User>
                        {
                            Code = ConstOperationCodes.INVALID_COUNTRY
                        };
                    }
                    //need to check if the phone number and email are for another user or not
                    user.Name = updatedUser.Name;
                    user.PhoneNumber = updatedUser.PhoneNumber;
                    user.Email = updatedUser.Email;
                    user.CountryIso = updatedUser.CountryIso;
                    user.ModifiedAt = DateTime.Now;
                    //context.Users.Update(user);
                    await context.SaveChangesAsync();
                    return new SuccessOperationResult<User>
                    {
                        Code = ConstOperationCodes.USER_UPDATED,
                        Result = user,
                    };
                }
                catch (Exception e)
                {
                    logger.Error($"Error updating user with id {updatedUser.Id} : {e.Message}");
                    return new FailedOperationResult<User>
                    {
                        Code = ConstOperationCodes.FAILED_OPERATION,
                    };
                }
            }
        }
    }
}
