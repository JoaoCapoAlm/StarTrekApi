using System.Net;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using static Application.Middleware.AppMiddleware;

namespace Application.Configurations
{
    public class AppException(string message,
        IDictionary<string, IEnumerable<string>> errors,
        HttpStatusCode statusCode = HttpStatusCode.BadRequest
    ) : Exception(message)
    {
        /// <summary>
        /// Validation errors
        /// </summary>
        public HttpStatusCode StatusCode { get; set; } = statusCode;
        internal IDictionary<string, IEnumerable<string>> Errors { get; set; } = errors;

        public AppException(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
            : this(message, new Dictionary<string, IEnumerable<string>>(), statusCode)
        {
        }

        public AppException(string message, IEnumerable<ValidationFailure> validationFailures)
            : this(message)
        {
            var mensagemPorPropriedade = validationFailures.GroupBy(e => e.PropertyName);
            Errors = mensagemPorPropriedade.ToDictionary(e => e.Key, e => e.Select(s => s.ErrorMessage));
        }
    }
}
