using CrossCutting.Exceptions;
using FluentValidation;

namespace CrossCutting.Extensions
{
    public static class FluentValidationExtension
    {
        public static void ValidateAndThrowStarTrek<T>(this IValidator<T> validator,
            T instance,
            string errorMessage)
        {
            var validation = validator.Validate(instance);

            if (!validation.IsValid)
                throw new AppException(errorMessage, validation.Errors);
        }

        public static async Task ValidateAndThrowAsyncStarTrek<T>(this IValidator<T> validator,
            T instance,
            string errorMessage)
        {
            var validation = await validator.ValidateAsync(instance);

            if (!validation.IsValid)
                throw new AppException(errorMessage, validation.Errors);
        }
    }
}
