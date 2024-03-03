using System.Net;
using FluentValidation.Results;

namespace CrossCutting.Exceptions
{
    public class AppException(string message,
        IDictionary<string, IEnumerable<string>> errors,
        HttpStatusCode statusCode = HttpStatusCode.BadRequest
    ) : Exception(message)
    {
        public HttpStatusCode StatusCode { get; set; } = statusCode;
        public IDictionary<string, IEnumerable<string>> Errors { get; set; } = errors;

        public AppException(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
            : this(message, new Dictionary<string, IEnumerable<string>>(), statusCode)
        {
        }

        public AppException(string message, IEnumerable<ValidationFailure> validationFailures)
            : this(message)
        {
            var errorsList = validationFailures.GroupBy(e => e.PropertyName);
            Errors = errorsList.ToDictionary(e => e.Key, e => e.Select(s => s.ErrorMessage));
        }
    }
}
