using AutoMapper;
using CrossCutting.Resources;
using Domain.DTOs;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Domain.Validation
{
    public class CreateCharacterValidation : AbstractValidator<CreateCharacterDto>
    {

        public CreateCharacterValidation(IStringLocalizer<Messages> localizerMessages, StarTrekContext context)
        {
            RuleFor(x => x.YearBirth)
                .GreaterThan((short)0)
                .When(x => x.YearBirth.HasValue)
                .WithMessage(localizerMessages["MustBeGreaterThanZero"]);

            RuleFor(x => x.MonthBirth)
                .InclusiveBetween((byte)1, (byte)12)
                .When(x => x.MonthBirth.HasValue)
                .WithMessage(localizerMessages["Invalid"]);

            RuleFor(x => x.DayBirth)
                .InclusiveBetween((byte)1, (byte)31)
                .When(x => x.DayBirth.HasValue)
                .WithMessage(localizerMessages["Invalid"]);

            RuleFor(x => x.YearDeath)
                .GreaterThan((short)0)
                .When(x => x.YearDeath.HasValue)
                .WithMessage(localizerMessages["MustBeGreaterThanZero"]);

            RuleFor(x => x.MonthDeath)
                .InclusiveBetween((byte)1, (byte)12)
                .When(x => x.MonthDeath.HasValue)
                .WithMessage(localizerMessages["Invalid"]);

            RuleFor(x => x.DayDeath)
                .InclusiveBetween((byte)1, (byte)31)
                .When(x => x.DayDeath.HasValue)
                .WithMessage(localizerMessages["Invalid"]);

            RuleFor(x => x.SpeciesId)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                    .WithMessage(localizerMessages["Required"])
                .MustAsync(async (id, cancellationToken) =>
                {
                    return await context.Species
                        .AsNoTracking()
                        .Where(x => x.SpeciesId.Equals(id))
                        .AnyAsync(cancellationToken);
                }).WithMessage(localizerMessages["NotFound"]);
        }
    }
}
