using System.Net;
using Application.Configurations;
using Azure.Core;
using Newtonsoft.Json;

namespace Application.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class AppMiddleware
    {
        private readonly RequestDelegate _next;

        public AppMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            } catch (Exception exception)
            {
                await HandleException(httpContext, exception);
            }
        }

        private static Task HandleException(HttpContext context, Exception exception)
        {
            var code = exception is ExceptionNofFound ? HttpStatusCode.NotFound : HttpStatusCode.InternalServerError;
            var result = JsonConvert.SerializeObject(new { error = exception.Message });

            context.Response.ContentType = ContentType.ApplicationJson.ToString();
            context.Response.StatusCode = code.GetHashCode();
            return context.Response.WriteAsync(result);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class AppMiddlewareExtensions
    {
        public static IApplicationBuilder UseAppMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AppMiddleware>();
        }
    }
}
