using Application.Data.Enum;
using Application.Helpers;
using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Data.Validation
{
    public class UpdateSerieValidation : AbstractValidator<UpdateSerieDto>
    {
        public UpdateSerieValidation(IStringLocalizer<Messages> localizer)
        {
            When(s => !string.IsNullOrEmpty(s.Abbreviation), () =>
            {
                RuleFor(s => s.Abbreviation)
                    .MaximumLength(3)
                    .WithMessage(localizer["MaximumSizeExceeded"])
                    .Must(RegexHelper.StringIsSimpleAlphabet)
                    .WithMessage(localizer["ShouldBeLettersWithoutAccents"]);
            });

            When(x => !string.IsNullOrWhiteSpace(x.ImdbId), () =>
            {
                RuleFor(m => m.ImdbId)
                    .Length(9, 12)
                        .WithMessage(localizer["InvalidLength"])
                    .Must(m => m.StartsWith("tt") && RegexHelper.StringIsNumeric(m[2..]))
                        .WithMessage(localizer["InvalidId"].Value);
            });

            When(s => !string.IsNullOrWhiteSpace(s.OriginalLanguageIso), () =>
            {
                RuleFor(s => s.OriginalLanguageIso)
                    .Must((dto, language) =>
                    {
                        var languageIso = RegexHelper.RemoveSpecialCharacters(language);
                        return System.Enum.IsDefined(typeof(LanguageEnum), languageIso);
                    }).WithMessage(localizer["LanguageCodeMustIso"].Value);
            });

            When(s => s.TimelineId.HasValue, () =>
            {
                RuleFor(s => s.TimelineId)
                    .IsInEnum()
                    .WithMessage(localizer["Invalid"].Value);
            });

            When(s => s.TmdbId.HasValue, () =>
            {
                RuleFor(s => s.TmdbId)
                    .GreaterThan(0)
                    .WithMessage(localizer["Invalid"].Value);
            });
        }
    }
}
