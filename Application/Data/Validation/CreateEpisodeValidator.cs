using Application.Helpers;
using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Data.Validation
{
    public class CreateEpisodeValidator : AbstractValidator<CreateEpisodeDto>
    {
        public CreateEpisodeValidator(IStringLocalizer<Messages> localizer)
        {
            When(x => !string.IsNullOrWhiteSpace(x.ImdbId), () =>
            {
                RuleFor(e => e.ImdbId)
                    .MinimumLength(9)
                        .WithMessage(localizer["Invalid"].Value)
                    .Must(e => e.StartsWith("tt") && RegexHelper.StringIsNumeric(e[2..]))
                        .WithMessage(localizer["Invalid"].Value);
            });

            RuleFor(e => e.Number)
                .GreaterThan(byte.MinValue)
                .WithMessage(localizer["MustBeGreaterThanZero"].Value);

            When(e => e.RealeaseDate.HasValue, () => {
                RuleFor(e => e.RealeaseDate)
                    .LessThan(DateOnly.FromDateTime(DateTime.Today));
            });

            When(e => e.StardateFrom.HasValue, () =>
            {
                RuleFor(e => e.StardateFrom)
                    .GreaterThan(0)
                    .WithMessage(localizer["MustBeGreaterThanZero"].Value);
            });

            When(e => e.StardateTo.HasValue, () =>
            {
                RuleFor(e => e.StardateTo)
                    .GreaterThan(float.NegativeZero)
                    .WithMessage(localizer["MustBeGreaterThanZero"].Value);
            });

            When(e => e.StardateFrom.HasValue && e.StardateTo.HasValue, () =>
            {
                RuleFor(e => e.StardateFrom)
                    .LessThanOrEqualTo(e => e.StardateTo)
                    .WithMessage("StardateFrom deve ser menor que StardateTo"); // TODO criar tradução
            });

            RuleFor(e => e.SynopsisResource)
                .NotEmpty()
                    .WithMessage(localizer["Required"].Value)
                .Must(RegexHelper.StringIsSimpleAlphabet)
                    .WithMessage(localizer["ShouldBeLettersWithoutAccents"].Value);

            RuleFor(e => e.Time)
                .GreaterThan(byte.MinValue)
                .WithMessage(localizer["MustBeGreaterThanZero"].Value);

            RuleFor(e => e.TitleResource)
                .NotEmpty()
                    .WithMessage(localizer["Required"].Value)
                .Must(RegexHelper.StringIsSimpleAlphabet)
                    .WithMessage(localizer["ShouldBeLettersWithoutAccents"].Value);
        }
    }
}
