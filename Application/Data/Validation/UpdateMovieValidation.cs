using Application.Data.Enum;
using Application.Helpers;
using Application.Resources;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Localization;
using NuGet.Protocol;

namespace Application.Data.Validation
{
    public class UpdateMovieValidation : AbstractValidator<UpdateMovieDto>
    {
        public UpdateMovieValidation(IStringLocalizer<Messages> localizer)
        {
            RuleFor(m => m.Time)
                .GreaterThan((short)0)
                .WithMessage(localizer["ValueGreaterThanZero"].Value);

            When(m => !string.IsNullOrWhiteSpace(m.ImdbId), () =>
            {
                RuleFor(m => m.ImdbId)
                    .Length(8, 14)
                        .WithMessage(localizer["Invalid"].Value)
                    .Must(m => m.StartsWith("tt") && RegexHelper.StringIsNumeric(m[2..]))
                        .WithMessage(localizer["Invalid"].Value);
            });

            RuleFor(m => m.TimelineId)
                .IsInEnum()
                .WithMessage(localizer["Invalid"].Value);

            When(x => x.ReleaseDate.HasValue, () =>
            {
                RuleFor(m => m.ReleaseDate)
                    .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
                    .WithMessage(localizer["Invalid"].Value);
            });

            When(m => !string.IsNullOrWhiteSpace(m.SynopsisResource), () =>
            {
                RuleFor(m => m.SynopsisResource)
                    .Must(RegexHelper.StringIsSimpleAlphabet)
                    .WithMessage($"{localizer["ShouldBeLettersWithoutAccents"].Value}");
            });

            When(m => !string.IsNullOrWhiteSpace(m.OriginalLanguageIso), () =>
            {
                RuleFor(x => x.OriginalLanguageIso)
                    .Must((dto, language) =>
                    {
                        var languageIso = RegexHelper.RemoveSpecialCharacters(language);
                        return System.Enum.IsDefined(typeof(LanguageEnum), languageIso);
                    }).WithMessage(localizer["LanguageCodeMustIso"].Value);
            });
        }
    }
}
