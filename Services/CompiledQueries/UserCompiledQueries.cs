using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.DbContexts;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Services.CompiledQueries
{
    public static class UserCompiledQueries
    {
        public static Func<CourseLibraryContext, Guid, Task<User>> GetUserById =
            EF.CompileAsyncQuery((CourseLibraryContext context , Guid id) =>
            context.Users.FirstOrDefault(u=>u.Id.Equals(id)));
        public static Func<CourseLibraryContext, string, Task<User>> GetUserByPhoneNumber =
            EF.CompileAsyncQuery((CourseLibraryContext context, string phone) =>
            context.Users.FirstOrDefault(u => u.PhoneNumber.Equals(phone)));
        public static Func<CourseLibraryContext, string, Task<User>> GetUserByEmail =
            EF.CompileAsyncQuery((CourseLibraryContext context, string email) => 
            context.Users.FirstOrDefault(u => u.Email.Equals(email)));
        public static Func<CourseLibraryContext, string, Task<bool>> IsPhoneNumberExists=
            EF.CompileAsyncQuery((CourseLibraryContext context, string phone) => 
            context.Users.Any(u => u.PhoneNumber.Equals(phone)));
        public static Func<CourseLibraryContext, string, Task<bool>> IsEmailExists =
            EF.CompileAsyncQuery((CourseLibraryContext context, string email) => 
            context.Users.Any(u => u.Email.Equals(email)));
        public static Func<CourseLibraryContext, string,string, Task<bool>> IsPhoneNumberOrEmailExists =
            EF.CompileAsyncQuery((CourseLibraryContext context, string phone,string email) =>
            context.Users.Any(u => u.PhoneNumber.Equals(phone) || u.Email.Equals(email)));
        public static Func<CourseLibraryContext, Guid, string, Task<bool>> IsEmailExistsForDifferentUser =
            EF.CompileAsyncQuery((CourseLibraryContext context, Guid id, string email) => context.Users.Any(u => u.Id != id && u.Email == email));
        public static Func<CourseLibraryContext, Guid, string, Task<bool>> IsPhoneExistsForDifferentUser =
            EF.CompileAsyncQuery((CourseLibraryContext context, Guid id, string phone) => context.Users.Any(u => u.Id != id && u.PhoneNumber == phone));
    }
}
