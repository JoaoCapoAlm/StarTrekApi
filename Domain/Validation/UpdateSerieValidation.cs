using CrossCutting.Enums;
using CrossCutting.Extensions;
using CrossCutting.Helpers;
using CrossCutting.Resources;
using Domain.DTOs;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Domain.Validation
{
    public class UpdateSerieValidation : AbstractValidator<UpdateSerieDto>
    {
        public UpdateSerieValidation(IStringLocalizer<Messages> localizer)
        {
            When(s => !string.IsNullOrEmpty(s.Abbreviation), () =>
            {
                RuleFor(s => s.Abbreviation)
                    .MaximumLength(3)
                    .WithMessage(localizer["MaximumLengthExceeded"])
                    .Must(RegexHelper.StringIsSimpleAlphabet)
                    .WithMessage(localizer["ShouldBeLettersWithoutAccents"]);
            });

            When(x => !string.IsNullOrWhiteSpace(x.ImdbId), () =>
            {
                RuleFor(m => m.ImdbId)
                    .Cascade(CascadeMode.Stop)
                    .ImdbValidation(localizer);
            });

            When(s => !string.IsNullOrWhiteSpace(s.OriginalLanguageIso), () =>
            {
                RuleFor(s => s.OriginalLanguageIso)
                    .Must((dto, language) =>
                    {
                        var languageIso = RegexHelper.RemoveSpecialCharacters(language);
                        return Enum.IsDefined(typeof(LanguageEnum), languageIso);
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
