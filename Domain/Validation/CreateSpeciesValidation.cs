using CrossCutting.Enums;
using CrossCutting.Helpers;
using CrossCutting.Resources;
using Domain.DTOs;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Domain.Validation
{
    public class CreateSpeciesValidation : AbstractValidator<CreateSpeciesDto>
    {
        public CreateSpeciesValidation(StarTrekContext context, IStringLocalizer<Messages> localizer)
        {
            RuleFor(x => x.ResourceName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                    .WithMessage(localizer["Required"].Value)
                .Must(RegexHelper.StringIsSimpleAlphabetOrNumber)
                    .WithMessage(localizer["ShouldBeLettersWithoutAccentsOrNumbers"].Value)
                .MustAsync(async (resource, cancellationToken) =>
                {
                    var checkExists = await context.Species
                        .AsNoTracking()
                        .Where(x => x.ResourceName.Equals(resource))
                        .AnyAsync(cancellationToken: cancellationToken);
                    return !checkExists;
                }).WithMessage(localizer["AlreadyExists"]);

            RuleFor(x => x.PlanetId)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                    .WithMessage(localizer["Required"].Value)
                .GreaterThan(byte.MinValue)
                    .WithMessage(localizer["MustBeGreaterThanZero"])
                .MustAsync(async (id, cancellationToken) =>
                {
                    var placeType = (byte)PlaceTypeEmum.Planet.GetHashCode();
                    var checkExists = await context.Place.AsNoTracking()
                        .Where(x => x.PlaceTypeId.Equals(placeType)
                            && x.PlaceId.Equals(id))
                        .AnyAsync(cancellationToken: cancellationToken);
                    
                    return checkExists;
                }).WithMessage(localizer["NotFound"]);
        }
    }
}
