using System.Net;
using static Application.Middleware.AppMiddleware;

namespace Application.Configurations
{
    public class AppException : Exception
    {
        /// <summary>
        /// Validation errors
        /// </summary>
        internal IEnumerable<ErrorContent> Errors { get; private set; }
        public HttpStatusCode StatusCode { get; set; }
        public AppException(string message) : this(message, Enumerable.Empty<ErrorContent>())
        {
        }

        public AppException(string message, IEnumerable<ErrorContent> errors, HttpStatusCode statusCode = HttpStatusCode.BadRequest) : base(message)
        {
            Errors = errors;
            StatusCode = statusCode;
        }
    }
}
