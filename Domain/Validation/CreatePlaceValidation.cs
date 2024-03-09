using CrossCutting.Enums;
using CrossCutting.Resources;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Domain.Validation
{
    public class CreatePlaceValidation : AbstractValidator<CreatePlaceDto>
    {
        public CreatePlaceValidation(StarTrekContext context, IStringLocalizer<Messages> localizer) {
            RuleFor(x => x.NameResource)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                    .WithMessage(localizer["Required"])
                .MustAsync(async (resource, cancellationToken) =>
                {
                    var checkExists = await context.vwResourcesPlaces.AsNoTracking()
                        .Where(x => x.Resource.Equals(resource))
                        .AnyAsync(cancellationToken);

                    return !checkExists;
                }).WithMessage(localizer["AlreadyExists"]);

            RuleFor(x => x.QuadrantId)
                .Must(x => Enum.IsDefined(typeof(QuadrantEnum), x))
                .WithMessage(localizer["Invalid"]);

            RuleFor(x => x.PlaceTypeId)
                .Must(x => Enum.IsDefined(typeof(PlaceTypeEmum), x))
                .WithMessage(localizer["Invalid"]);
        }
    }
}
