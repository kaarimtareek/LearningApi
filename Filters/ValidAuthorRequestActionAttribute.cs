using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.DbContexts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Services.CompiledQueries;
using Services.OperationCodes;
using Services.ResultObject;

namespace Filters
{
    public class ValidAuthorRequestActionAttribute : IAsyncActionFilter
    {
        private readonly DbContextOptions<CourseLibraryContext> options;
        //just for testing filters
        public ValidAuthorRequestActionAttribute(DbContextOptions<CourseLibraryContext> options)
        {
            this.options = options;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var parameter = context.ActionArguments.Values.SingleOrDefault(v => v is Guid);
            if(parameter == null)
            {
                context.Result = new BadRequestObjectResult(new FailedOperationResult<Author>
                {
                    Code = ConstOperationCodes.FAILED_OPERATION,
                    Message = "Invalid request"
                });
                return;
            }
            
            await next();
        }
    }
}
