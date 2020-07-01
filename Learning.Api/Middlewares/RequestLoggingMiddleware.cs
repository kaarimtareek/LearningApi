using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Services.LoggerService;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Learning.Api.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerService _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILoggerService loggerFactory)
        {
            _next = next;
            _logger = loggerFactory;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                var requestBodyStream = new MemoryStream();
                var originalRequestBody = context.Request.Body;

                //get the content of the request body to stream
                await context.Request.Body.CopyToAsync(requestBodyStream);
                //go the beggining of the stream
                requestBodyStream.Seek(0, SeekOrigin.Begin);
                //to get the url from the request 
                var url = UriHelper.GetDisplayUrl(context.Request);
                //read the stream to the end
                var requestBodyText = new StreamReader(requestBodyStream).ReadToEnd();
                //get the stream back to origin
                requestBodyStream.Seek(0, SeekOrigin.Begin);
                _logger.Info($"Request method :{context.Request?.Method} Url :{url} , Query String: {context.Request?.QueryString} ,Body : ${requestBodyText}");
                //return the stream back to the request body
                context.Request.Body = requestBodyStream;
                await _next(context);
                
                context.Request.Body = originalRequestBody;

            }catch(Exception e)
            {
                _logger.Error($"Error in Request Logging Middleware :{e.Message}");
            }
            
        }
    }
}
