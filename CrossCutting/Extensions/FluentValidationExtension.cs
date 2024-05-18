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
            IStringLocalizer<Messages> localizer,
            bool isPerson = false
        )
        {
            var validation = ruleBuilder
                .Must(RegexHelper.IsValidImdbId)
                    .WithMessage(localizer["Invalid"]);

            if (isPerson)
                return validation.Must(e => e.StartsWith("nn"))
                        .WithMessage("Deve ser informado o ID de uma pessoa");  // TODO translate
            else
                return validation.Must(e => e.StartsWith("tt"))
                    .WithMessage("Não deve ser informado o ID de uma pessoa"); // TODO translate
        }
    } 
}
