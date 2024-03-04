using CrossCutting.Exceptions;
using CrossCutting.Helpers;
using CrossCutting.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

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

        public static IRuleBuilderOptions<T, string> ImdbValidation<T>(
            this IRuleBuilder<T, string> ruleBuilder,
            IStringLocalizer<Messages> localizer
        )
        {
            return ruleBuilder
                .Length(8, 13)
                    .WithMessage(localizer["InvalidLength"])
                .Must(e => e.StartsWith("tt") && RegexHelper.StringIsNumeric(e[2..]))
                    .WithMessage(localizer["Invalid"]);
        }
    }
}
