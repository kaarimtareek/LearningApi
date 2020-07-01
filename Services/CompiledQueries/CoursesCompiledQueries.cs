using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Data.DbContexts;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Services.CompiledQueries
{
    public static class CoursesCompiledQueries
    {
        public static readonly Func<CourseLibraryContext, Guid, Task<List<Course>>> GetCoursesByAuthorId =
            EF.CompileAsyncQuery((CourseLibraryContext context, Guid authorId) => context.Courses.Where(c => c.AuthorId.Equals(authorId)).ToList());

        public static readonly Func<CourseLibraryContext, Guid, Guid, Task<Course>> GetCourseByIdWithAuthorId =
            EF.CompileAsyncQuery((CourseLibraryContext context, Guid authorId, Guid courseId) => context.Courses
            .SingleOrDefault(c => c.AuthorId.Equals(authorId) && c.Id.Equals(courseId)));
        public static readonly Func<CourseLibraryContext, Guid, Task<Course>> GetCourseById =
            EF.CompileAsyncQuery((CourseLibraryContext context, Guid courseId) => context.Courses
            .SingleOrDefault(c => c.Id.Equals(courseId)));
        public static readonly Func<CourseLibraryContext, Guid, Task<bool>> IsCourseExist =
            EF.CompileAsyncQuery((CourseLibraryContext context,Guid courseId)=> context.Courses.Any(c=>c.Id == courseId));
        public static readonly Func<CourseLibraryContext, Guid,Guid, Task<bool>> IsCourseExistForAuthor =
            EF.CompileAsyncQuery((CourseLibraryContext context, Guid courseId,Guid authorId) => context.Courses.Any(c => c.Id == courseId && c.AuthorId == authorId));
    }
}
