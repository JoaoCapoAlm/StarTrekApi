using CrossCutting.Exceptions;
using FluentValidation;

namespace CrossCutting.Extensions
{
    public static class FluentValidationExtension
    {
        public static void ValidateAndThrowAppException<T>(this IValidator<T> validator,
            T instance,
            string errorMessage)
        {
            var validation = validator.Validate(instance);

            if (!validation.IsValid)
                throw new AppException(errorMessage, validation.Errors);
        }

        public static async Task ValidateAndThrowAsyncAppException<T>(this IValidator<T> validator,
            T instance,
            string errorMessage)
        {
            var validation = await validator.ValidateAsync(instance);

            if (!validation.IsValid)
                throw new AppException(errorMessage, validation.Errors);
        }
    }
}
