using AutoMapper;
using CrossCutting.Resources;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Domain.Validation
{
    public class CreateEpisodeWithSeasonIdValidation : AbstractValidator<CreateEpisodeWithSeasonIdDto>
    {
        public CreateEpisodeWithSeasonIdValidation(
            IMapper mapper,
            IStringLocalizer<Messages> localizerMessages,
            StarTrekContext context)
        {
            RuleFor(x => x.SeasonId)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                    .WithMessage(localizerMessages["Required"])
                .GreaterThan(0)
                    .WithMessage(localizerMessages["MustBeGreaterThanZero"])
                .MustAsync(async (id, cancellationToken) =>
                {
                    return await context.Season.AsNoTracking()
                        .Where(x => x.SeasonId.Equals(id))
                        .AnyAsync(cancellationToken: cancellationToken);
                }).WithMessage(localizerMessages["NotFound"])
                .DependentRules(() =>
                {
                    RuleFor(x => x.TitleResource)
                        .MustAsync(async (dto, resource, cancellationoken) =>
                        {
                            var season = await context.Season.AsNoTracking()
                                .Where(x => x.SeasonId.Equals(dto.SeasonId))
                                .FirstAsync(cancellationToken: cancellationoken);

                            return resource.StartsWith(season.Serie.Abbreviation);
                        }).WithMessage(localizerMessages["MustStartWithTheSeriesAbbreviation"]);

                    RuleFor(x => x.Number)
                        .MustAsync(async (dto, number, cancellationToken) =>
                        {
                            var checkEpExists = await context.Episode.AsNoTracking()
                                .Where(x => x.SeasonId.Equals(dto.SeasonId) && x.Number.Equals(number))
                                .AnyAsync(cancellationToken: cancellationToken);

                            return !checkEpExists;
                        }).WithMessage(localizerMessages["EpisodeAlreadyRegistered"]);
                });

            RuleFor(x => mapper.Map<CreateEpisodeDto>(x))
                .SetValidator(x => new CreateEpisodeValidator(localizerMessages, context));
        }
    }
}
