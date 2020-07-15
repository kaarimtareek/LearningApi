using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Data.DbContexts;
using Filters;
using Learning.Api.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
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
            var key = Encoding.ASCII.GetBytes(Configuration.GetSection("Jwt:Key").Value);
            services.AddAuthentication(
                x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }
                )
                .AddJwtBearer(
                Options =>
                {
                    Options.RequireHttpsMetadata = false;
                    Options.SaveToken = true;
                    Options.TokenValidationParameters = new TokenValidationParameters
                    {
                       
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                       RequireExpirationTime = true,

                    };
                }
            );
              
            services.AddSingleton<IPropertyMappingService, PropertyMappingService>();
            services.AddSingleton<IFilterationService, FilterationService>();
            services.AddSingleton<ICourseLibraryService, CourseLibraryService>();
            services.AddSingleton<ILoggerService, LoggerService>();
            services.AddScoped<ValidAuthorRequestActionAttribute>();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMiddleware<RequestLoggingMiddleware>();
            app.UseMiddleware<ResponseLoggingMiddleware>();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
           
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
