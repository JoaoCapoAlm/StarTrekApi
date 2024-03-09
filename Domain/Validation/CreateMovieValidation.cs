using CrossCutting.Enums;
using CrossCutting.Extensions;
using CrossCutting.Helpers;
using CrossCutting.Resources;
using Domain.DTOs;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Domain.Validation
{
    public class CreateMovieValidation : AbstractValidator<CreateMovieDto>
    {
        public CreateMovieValidation(IStringLocalizer<Messages> localizer)
        {
            When(x => !string.IsNullOrWhiteSpace(x.ImdbId), () =>
            {
                RuleFor(m => m.ImdbId)
                    .Cascade(CascadeMode.Stop)
                    .ImdbValidation(localizer);
            });

            RuleFor(x => x.OriginalLanguageIso)
                .NotEmpty()
                    .WithMessage(localizer["Required"].Value)
                .Must(language =>
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
