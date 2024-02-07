using Application.Data.Enum;
using Application.Helpers;
using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Data.Validation
{
    public class CreateSerieValidation : AbstractValidator<CreateSerieDto>
    {
        public CreateSerieValidation(IStringLocalizer<Messages> localizer)
        {
            RuleFor(s => s.Abbreviation)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                    .WithMessage(localizer["Required"].Value)
                .Length(3)
                    .WithMessage(localizer["Invalid"].Value);

            When(x => !string.IsNullOrWhiteSpace(x.ImdbId), () =>
            {
                RuleFor(m => m.ImdbId)
                    .MinimumLength(9)
                        .WithMessage(localizer["Invalid"].Value)
                    .Must(m => m.StartsWith("tt") && RegexHelper.StringIsNumeric(m[2..]))
                        .WithMessage(localizer["Invalid"].Value);
            });

            RuleFor(s => s.OriginalLanguageIso)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                    .WithMessage(localizer["Required"].Value)
                .Must((dto, language) =>
                {
                    var languageIso = RegexHelper.RemoveSpecialCharacters(language);
                    return System.Enum.IsDefined(typeof(LanguageEnum), languageIso);
                }).WithMessage(localizer["LanguageCodeMustIso"].Value);

            RuleFor(s => s.OriginalName)
                .NotEmpty()
                .WithMessage(localizer["Required"].Value);

            RuleForEach(s => s.Seasons)
                .SetValidator(new CreateSeasonValidation(localizer));

            RuleFor(s => s.SynopsisResource)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                    .WithMessage(localizer["Required"].Value)
                .Must(RegexHelper.StringIsSimpleAlphabet)
                    .WithMessage(localizer["ShouldBeLettersWithoutAccents"].Value);

            RuleFor(s => s.TimelineId)
                .IsInEnum()
                .WithMessage(localizer["Invalid"].Value);

            RuleFor(s => s.TmdbId)
                .GreaterThan(0)
                .WithMessage(localizer["Invalid"].Value);
        }
    }
}
