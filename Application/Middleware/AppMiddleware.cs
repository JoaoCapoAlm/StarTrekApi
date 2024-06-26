﻿using System.Net;
using Azure.Core;
using CrossCutting.AppModel;
using CrossCutting.Exceptions;
using FluentValidation;
using Newtonsoft.Json;

namespace Application.Middleware
{
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
                UnauthorizedAccessException => HttpStatusCode.Unauthorized,
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
