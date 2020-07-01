using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Services.LoggerService;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Learning.Api.Middlewares
{
    public class ResponseLoggingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILoggerService logger;
        public ResponseLoggingMiddleware(RequestDelegate next,ILoggerService logger)
        {
            this.next = next;
            this.logger = logger;
        }
        public async Task Invoke (HttpContext context)
        {
            try
            {
                //why the code in that order?
                int statusCode = context.Response.StatusCode;
                MemoryStream bodyStream = new MemoryStream();
                var originalBodyStream = context.Response.Body;
                //why
                context.Response.Body = bodyStream;
                await next(context);
                var url = UriHelper.GetDisplayUrl(context.Request);
                bodyStream.Seek(0, SeekOrigin.Begin);
                var responseBody = new StreamReader(bodyStream).ReadToEnd();
                logger.Info($"Response For url : {url} , body :{responseBody} , status code :{statusCode}");
                bodyStream.Seek(0, SeekOrigin.Begin);
                await bodyStream.CopyToAsync(originalBodyStream);
              
            }
            catch(Exception e)
            {
                logger.Error($"Error in Response Logging Middleware :{e.Message}");
            }
        }
    }
}
