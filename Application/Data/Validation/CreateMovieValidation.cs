using Application.Data.Enum;
using Application.Helpers;
using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json.Linq;

namespace Application.Data.Validation
{
    public class CreateMovieValidation : AbstractValidator<CreateMovieDto>
    {
        public CreateMovieValidation(IStringLocalizer<Messages> localizer)
        {
            RuleFor(m => m.SynopsisResource)
                .NotEmpty()
                .WithMessage(localizer["Required"].Value)
                .Must(c => c.Contains(' ') == false)
                .WithMessage(localizer["CannotContainSpace"].Value);

            RuleFor(m => m.Time)
                .NotEmpty()
                .GreaterThan(byte.MinValue)
                .WithMessage(localizer["Invalid"].Value);

            RuleFor(m => m.TimelineId)
                .IsInEnum()
                .WithMessage(localizer["Invalid"].Value);

            RuleFor(x => x.OriginalLanguageIso)
                .NotEmpty()
                .WithMessage(localizer["Required"].Value);

            When(x => x.ReleaseDate.HasValue, () =>
            {
                RuleFor(m => m.ReleaseDate.Value)
                    .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
                    .WithMessage(localizer["Invalid"].Value);
            });

            When(x => !string.IsNullOrWhiteSpace(x.OriginalLanguageIso), () =>
            {
                RuleFor(x => x.OriginalLanguageIso).Custom((value, context) =>
                {
                    var languageIso = RegexHelper.RemoveSpecialCharacters(value);
                    if (!System.Enum.IsDefined(typeof(LanguageEnum), languageIso))
                        context.AddFailure(localizer["Invalid"].Value);
                });

            });

            When(x => !string.IsNullOrWhiteSpace(x.ImdbId), () =>
            {
                RuleFor(m => m.ImdbId)
                    .Must(m => m.StartsWith("tt") && m.Length <= 11)
                    .WithMessage(localizer["Invalid"].Value);
            });
        }

    }
}
