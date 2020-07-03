using Data.DbContexts;
using DTOs.AuthorDTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore.Query;
using Entities;

namespace Services.CompiledQueries
{
    public static class AuthorCompiledQueries
    {
        //TODO:complete course queries ,services and controllers
        public static readonly Func<CourseLibraryContext, IEnumerable<Author>> GetAuthors =
            EF.CompileQuery(
                (CourseLibraryContext libraryContext) => libraryContext.Authors);
        public static readonly Func<CourseLibraryContext, Guid, Task<Author>> GetAuthor = 
            EF.CompileAsyncQuery((CourseLibraryContext libraryContext, Guid id) =>
            //why cant make it async?
            libraryContext.Authors.SingleOrDefault(author => author.Id == id)
            );

        public static readonly Func<CourseLibraryContext, Guid, Task<Author>> GetAuthorWithCourses =
            EF.CompileAsyncQuery(
                (CourseLibraryContext context, Guid authorId) =>
                context.Authors.Include(author=>author.Courses).SingleOrDefault(author => author.Id == authorId));

        public static readonly Func<CourseLibraryContext, string, Task<List<Author>>> GetAuthorsByName =
           EF.CompileAsyncQuery((CourseLibraryContext context,string name)=>
           context.Authors.Where(author=>author.FirstName.Contains(name)||author.LastName.Contains(name)).ToList());

        public static readonly Func<CourseLibraryContext, string, string, Task<bool>> IsExistSameAuthorName =
            EF.CompileAsyncQuery((CourseLibraryContext context, string firstName, string lastName) => context.Authors.Any(a => a.FirstName == firstName && a.LastName == lastName));
        public static readonly Func<CourseLibraryContext, Guid, Task<bool>> IsAuthorExist =
            EF.CompileAsyncQuery((CourseLibraryContext context,Guid id)=> context.Authors.Any(a=>a.Id == id));
    }
}
