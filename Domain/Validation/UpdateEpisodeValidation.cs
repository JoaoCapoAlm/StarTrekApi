using CrossCutting.Extensions;
using CrossCutting.Resources;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Domain.Validation
{
    public class UpdateEpisodeValidation : AbstractValidator<UpdateEpisodeDto>
    {
        public UpdateEpisodeValidation(IStringLocalizer<Messages> localizer, StarTrekContext context)
        {
            When((x, cancellationToken) => !string.IsNullOrEmpty(x.ImdbId), () =>
            {
                RuleFor(x => x.ImdbId)
                    .Cascade(CascadeMode.Stop)
                    .ImdbValidation(localizer)
                    .MustAsync(async (imdb, cancellationToken) =>
                    {
                        var checkExists = await context.vwImdb
                            .Where(x => x.Equals(imdb))
                            .AnyAsync(cancellationToken: cancellationToken);

                        return !checkExists;
                    }).WithMessage(localizer["AlreadyExists"]);
            });

            When(x => x.Number.HasValue, () =>
            {
                RuleFor(x => x.Number)
                    .GreaterThan(byte.MinValue)
                    .WithMessage(localizer["MustBeGreaterThanZero"]);
            });

            When(x => x.RealeaseDate.HasValue, () =>
            {
                RuleFor(x => x.RealeaseDate)
                    .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
                    .WithMessage(localizer["ItCannotBeaFutureDate"]);
            });

            When(x => x.SeasonId.HasValue, () =>
            {
                RuleFor(x => x.SeasonId)
                    .GreaterThan((short)0)
                        .WithMessage(localizer["MustBeGreaterThanZero"]);
            });

            When(x => x.StardateFrom.HasValue, () =>
            {
                RuleFor(x => x.StardateFrom)
                    .GreaterThanOrEqualTo(1000)
                    .WithMessage(localizer["Invalid"]);
            });

            When(x => x.StardateTo.HasValue, () =>
            {
                RuleFor(x => x.StardateTo)
                    .GreaterThanOrEqualTo(1000)
                    .WithMessage(localizer["Invalid"]);
            });

            When(x => x.StardateFrom.HasValue && x.StardateTo.HasValue, () =>
            {
                RuleFor(x => x.StardateFrom)
                    .LessThanOrEqualTo(x => x.StardateTo)
                    .WithMessage(localizer["StardateFromLessThanOrEqualStardateTo"]);
            });

            When(x => x.Time.HasValue, () =>
            {
                RuleFor(x => x.Time)
                    .NotEmpty()
                    .WithMessage(localizer["Required"]);
            });
        }
    }
}
