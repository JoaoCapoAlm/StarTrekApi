using CrossCutting.Extensions;
using CrossCutting.Helpers;
using CrossCutting.Resources;
using Domain.DTOs;
using Domain.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Domain.Validation
{
    public class CreateEpisodeValidator : AbstractValidator<CreateEpisodeDto>
    {
        public CreateEpisodeValidator(IStringLocalizer<Messages> localizer, StarTrekContext context)
        {
            var viewsRepository = new ViewsRepository(context);

            When(x => !string.IsNullOrWhiteSpace(x.ImdbId), () =>
            {
                RuleFor(e => e.ImdbId)
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

            RuleFor(e => e.Number)
                .GreaterThan(byte.MinValue)
                .WithMessage(localizer["MustBeGreaterThanZero"].Value);

            When(e => e.RealeaseDate.HasValue, () =>
            {
                RuleFor(e => e.RealeaseDate)
                    .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
                    .WithMessage(localizer["ItCannotBeaFutureDate"]);
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
                    .WithMessage(localizer["StardateFromLessThanOrEqualStardateTo"]);
            });

            RuleFor(e => e.Time)
                .GreaterThan(byte.MinValue)
                .WithMessage(localizer["MustBeGreaterThanZero"].Value);

            RuleFor(e => e.TitleResource)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                    .WithMessage(localizer["Required"].Value)
                .Must(RegexHelper.StringIsSimpleAlphabetOrNumber)
                    .WithMessage(localizer["ShouldBeLettersWithoutAccentsOrNumbers"].Value)
                .Must(x => !x.EndsWith("Synopsis", StringComparison.CurrentCultureIgnoreCase))
                    .WithMessage(localizer["MustNotContainSynopsisAtTheEnd"])
                .MustAsync(async (resource, cancellationToken) =>
                {
                    var checkExists = await viewsRepository.CheckResourceExists(resource, cancellationToken: cancellationToken);
                    return !checkExists;
                }).WithMessage(localizer["AlreadyExists"]);
        }
    }
}
