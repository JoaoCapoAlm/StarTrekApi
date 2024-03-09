using AutoMapper;
using CrossCutting.Resources;
using Domain.DTOs;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Domain.Validation
{
    public class CreateSeasonWithSerieIdValidation : AbstractValidator<CreateSeasonWithSerieIdDto>
    {
        public CreateSeasonWithSerieIdValidation(IMapper mapper,
            IStringLocalizer<Messages> localizerMessages,
            StarTrekContext context)
        {
            RuleFor(x => mapper.Map<CreateSeasonDto>(x))
                .SetValidator(x => new CreateSeasonValidation(localizerMessages, context));

            RuleFor(x => x.SerieId)
                .NotEmpty()
                    .WithMessage(localizerMessages["MustBeGreaterThanZero"])
                .MustAsync(async (id, cancellationToken) =>
                {
                    return await context.Season.AsNoTracking()
                        .Where(x => x.SeasonId.Equals(id))
                        .AnyAsync(cancellationToken: cancellationToken);
                }).WithMessage(localizerMessages["NotFound"])
                .DependentRules(() =>
                {
                    RuleForEach(x => x.Episodes)
                        .MustAsync(async (seasonDto, episodeDto, cancellationoken) =>
                        {
                            var serie = await context.Serie.AsNoTracking()
                                .Where(x => x.SerieId.Equals(seasonDto.SerieId))
                                .FirstAsync(cancellationToken: cancellationoken);

                            return episodeDto.TitleResource.StartsWith(serie.Abbreviation, StringComparison.CurrentCultureIgnoreCase);
                        }).WithMessage(localizerMessages["MustStartWithTheSeriesAbbreviation"]);

                    RuleFor(x => x.Number)
                        .MustAsync(async (seasonDto, number, cancellationToken) =>
                        {
                            var checkExists = await context.Episode.AsNoTracking()
                                .Where(x => x.SeasonId.Equals(seasonDto.SerieId) && x.Number.Equals(number))
                                .AnyAsync(cancellationToken: cancellationToken);

                            return !checkExists;
                        }).WithMessage(localizerMessages["SeasonAlreadyRegistered"]);
                });
        }
    }
}
