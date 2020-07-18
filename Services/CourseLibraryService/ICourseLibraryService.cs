using Data.Entities;
using DTOs.QueryParamters;
using Entities;
using Services.PaginationService;
using Services.ResultObject;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.CourseLibraryService
{
   public interface ICourseLibraryService
    {
        OperationResult<IEnumerable<Author>> GetAuthors();
        Task<OperationResult<Author>> GetAuthor(Guid id,bool withCourses =false);
  
        OperationResult<PaginationList<Author>> GetAuthors(AuthorsQueryParamters queryParamters);
        Task<OperationResult<Author>> AddAuthor(Author author);
        Task<OperationResult<Author>> UpdateAuthor(Author author);
        Task<OperationResult<PaginationList<Course>>> GetCourses(CoursesQueryParameters queryParameters);
        Task<OperationResult<IEnumerable<Course>>> GetCoursesForAuthor(Guid authorId);
        OperationResult<PaginationList<Course>> GetCourses(CoursesQueryParameters queryParameters, Guid authorId);
        Task<OperationResult<Course>> GetCourseById(Guid courseId);
        Task<OperationResult<Course>> AddCourse(Course course);
        Task<OperationResult<Course>> UpdateCourse(Course course);
        Task<bool> IsCourseExist(Guid courseId);
        Task<OperationResult<bool>> DeleteCourse(Guid courseId);
        Task<OperationResult<User>> AddUser(User user);
        Task<OperationResult<User>> GetUserById(Guid id);
        Task<OperationResult<bool>> DeleteUser(Guid Id);
        Task<OperationResult<User>> UpdateUser(User user);

    }
}
