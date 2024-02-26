using Application.Helpers;
using Application.Repositories;
using Application.Resources;
using Domain;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Data.Validation
{
    public class CreateEpisodeValidator : AbstractValidator<CreateEpisodeDto>
    {
        public CreateEpisodeValidator(IStringLocalizer<Messages> localizer, StarTrekContext context)
        {
            var viewsRepository = new ViewsRepository(context);

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

            When(e => e.RealeaseDate.HasValue, () =>
            {
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
                    .WithMessage(localizer["StardateFromLessThanOrEqualStardateTo"]);
            });

            RuleFor(e => e.SynopsisResource)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                    .WithMessage(localizer["Required"].Value)
                .Must(RegexHelper.StringIsSimpleAlphabetOrNumber)
                    .WithMessage(localizer["ShouldBeLettersWithoutAccentsOrNumbers"].Value)
                .Must(x => x.EndsWith("Synopsis"))
                    .WithMessage(localizer["MustContainSynopsisAtTheEnd"])
                .MustAsync(async (resource, cancellationToken) =>
                {
                    var checkExists = await viewsRepository.CheckResourceExists(resource, cancellationToken: cancellationToken);
                    return !checkExists;
                }).WithMessage(localizer["AlreadyExists"]);

            RuleFor(e => e.Time)
                .GreaterThan(byte.MinValue)
                .WithMessage(localizer["MustBeGreaterThanZero"].Value);

            RuleFor(e => e.TitleResource)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                    .WithMessage(localizer["Required"].Value)
                .Must(RegexHelper.StringIsSimpleAlphabetOrNumber)
                    .WithMessage(localizer["ShouldBeLettersWithoutAccentsOrNumbers"].Value)
                .Must(x => !x.EndsWith("Synopsis"))
                    .WithMessage(localizer["MustNotContainSynopsisAtTheEnd"])
                .MustAsync(async (resource, cancellationToken) =>
                {
                    var checkExists = await viewsRepository.CheckResourceExists(resource, cancellationToken: cancellationToken);
                    return !checkExists;
                }).WithMessage(localizer["AlreadyExists"]);
        }
    }
}
