using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Data.DbContexts;
using Learning.Api.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Services.CourseLibraryService;
using Services.FilterationService;
using Services.LoggerService;
using Services.PropertyMappingService;

namespace Learning.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper( AppDomain.CurrentDomain.GetAssemblies());
            services.AddDbContextPool<CourseLibraryContext>(
                options =>
                options.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=LearningDb;Trusted_Connection=True;")
                );
            services.AddSingleton<IPropertyMappingService, PropertyMappingService>();
            services.AddSingleton<IFilterationService, FilterationService>();
            services.AddSingleton<ICourseLibraryService, CourseLibraryService>();
            services.AddSingleton<ILoggerService, LoggerService>();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
           
            app.UseRouting();

            app.UseAuthorization();
            app.UseMiddleware<RequestLoggingMiddleware>();
            app.UseMiddleware<ResponseLoggingMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
