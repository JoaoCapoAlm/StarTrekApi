using Application.Data.Enum;
using Application.Helpers;
using Application.Resources;
using FluentValidation;
using Humanizer;
using Microsoft.Extensions.Localization;

namespace Application.Data.Validation
{
    public class UpdateMovieValidation : AbstractValidator<UpdateMovieDto>
    {
        public UpdateMovieValidation(IStringLocalizer<Messages> localizer) // TODO: verificar se aplicar apenas caso tenha valor
        {
            RuleFor(m => m.Time)
                .GreaterThan((short)0)
                .WithMessage(localizer["Invalid"].Value);

            RuleFor(m => m.ImdbId)
                .Must(m => m.StartsWith("tt"))
                .WithMessage(localizer["Invalid"].Value);

            RuleFor(m => m.TimelineId).Must(m => m.Value == 1 || m.Value == 2)
                .WithMessage(localizer["Invalid"].Value);

            RuleFor(m => m.ReleaseDate)
                .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
                .WithMessage(localizer["Invalid"].Value);

            RuleFor(m => m.OriginalLanguageIso); // TODO: adicionar validação para ver se existe no ENUM
            When(m => !string.IsNullOrWhiteSpace(m.SynopsisResource), () =>
            {
                RuleFor(m => m.SynopsisResource)
                    .Matches(@"\A\S+\z")
                    .WithMessage($"{localizer["Invalid"].Value} - {localizer["CannotContainSpace"].Value}");
            });

            When(m => !string.IsNullOrWhiteSpace(m.OriginalLanguageIso), () =>
            {
                RuleFor(m => RegexHelper.RemoveSpecialCharacters(m.OriginalLanguageIso))
                    .IsEnumName(typeof(LanguageEnum), false)
                    .WithMessage(localizer["Invalid"].Value);
            });

        }
    }
}
