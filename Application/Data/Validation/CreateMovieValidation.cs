using Application.Data;
using Application.Data.Enums;
using Application.Helpers;
using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Data.Validation
{
    public class CreateMovieValidation : AbstractValidator<CreateMovieDto>
    {
        public CreateMovieValidation(IStringLocalizer<Messages> localizer)
        {
            When(x => !string.IsNullOrWhiteSpace(x.ImdbId), () =>
            {
                RuleFor(m => m.ImdbId)
                    .MinimumLength(9)
                        .WithMessage(localizer["Invalid"].Value)
                    .Must(m => m.StartsWith("tt") && RegexHelper.StringIsNumeric(m[2..]))
                        .WithMessage(localizer["Invalid"].Value);
            });

            RuleFor(x => x.OriginalLanguageIso)
                .NotEmpty()
                    .WithMessage(localizer["Required"].Value)
                .Must((dto, language) =>
                {
                    var languageIso = RegexHelper.RemoveSpecialCharacters(language);
                    return Enum.IsDefined(typeof(LanguageEnum), languageIso);
                }).WithMessage(localizer["LanguageCodeMustIso"].Value);

            RuleFor(m => m.Time)
                .NotEmpty().WithMessage(localizer["ValueGreaterThanZero"].Value);

            RuleFor(m => m.TimelineId)
                .IsInEnum()
                .WithMessage(localizer["Invalid"].Value);

            RuleFor(m => m.TitleResource)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                    .WithMessage(localizer["Required"].Value)
                .Must(RegexHelper.StringIsSimpleAlphabetOrNumber)
                    .WithMessage(localizer["ShouldBeLettersWithoutAccentsOrNumbers"].Value);

            RuleFor(m => m.SynopsisResource)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                    .WithMessage(localizer["Required"].Value)
                .Must(RegexHelper.StringIsSimpleAlphabetOrNumber)
                    .WithMessage(localizer["ShouldBeLettersWithoutAccentsOrNumbers"].Value);

            When(x => x.ReleaseDate.HasValue, () =>
            {
                RuleFor(m => m.ReleaseDate)
                    .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
                    .WithMessage(localizer["Invalid"].Value);
            });
        }
    }
}
