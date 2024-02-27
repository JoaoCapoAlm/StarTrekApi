using System.Net;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using static Application.Middleware.AppMiddleware;

namespace Application.Configurations
{
    public class AppException : Exception
    {
        /// <summary>
        /// Validation errors
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }
        internal IDictionary<string, IEnumerable<string>> Errors { get; set; }

        public AppException(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
            : this(message, new Dictionary<string, IEnumerable<string>>(), statusCode)
        {
        }

        public AppException(string message,
            IDictionary<string, IEnumerable<string>> errors,
            HttpStatusCode statusCode = HttpStatusCode.BadRequest
            ) : base(message)
        {
            Errors = errors;
            StatusCode = statusCode;
        }

        public AppException(string message, IEnumerable<ValidationFailure> validationFailures)
            : this(message)
        {
            var mensagemPorPropriedade = validationFailures.GroupBy(e => e.PropertyName);
            Errors = mensagemPorPropriedade.ToDictionary(e => e.Key, e => e.Select(s => s.ErrorMessage));
        }
    }
}
