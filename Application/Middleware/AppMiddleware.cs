﻿using System.Net;
using Azure.Core;
using CrossCutting.Exceptions;
using FluentValidation;
using Newtonsoft.Json;

namespace Application.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class AppMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception exception)
            {
                await HandleException(httpContext, exception);
            }
        }

        private static Task HandleException(HttpContext context, Exception exception)
        {
            var code = exception switch
            {
                ArgumentException => HttpStatusCode.BadRequest,
                ValidationException => HttpStatusCode.BadRequest,
                _ => HttpStatusCode.InternalServerError
            };

            bool isProduction = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == Environments.Production;

            string message = exception.Message;
            if (isProduction && code.Equals(HttpStatusCode.InternalServerError))
                message = "Internal Server Error";
            else if (!isProduction && exception.InnerException != null)
                message = $"{exception.Message} - {exception.InnerException}";

            var responseBody = new ContentResponse()
            {
                title = message,
                errors = new Dictionary<string, IEnumerable<string>>()
            };

            if (exception.GetType().Equals(typeof(AppException)))
            {
                var appEx = (AppException)exception;
                code = appEx.StatusCode;
                responseBody.errors = appEx.Errors;
            }

            context.Response.ContentType = ContentType.ApplicationJson.ToString();
            context.Response.StatusCode = code.GetHashCode();

            return context.Response.WriteAsync(JsonConvert.SerializeObject(responseBody));
        }

        internal class ContentResponse
        {
#pragma warning disable IDE1006 // Naming Styles
            public string title { get; set; }
            public IDictionary<string, IEnumerable<string>> errors { get; set; }
#pragma warning restore IDE1006 // Naming Styles
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
