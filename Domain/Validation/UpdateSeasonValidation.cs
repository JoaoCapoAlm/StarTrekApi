using CrossCutting.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Domain.Validation
{
    public class UpdateSeasonValidation : AbstractValidator<UpdateSeasonDto>
    {
        public UpdateSeasonValidation(IStringLocalizer<Messages> localizerMessages)
        {
            When(x => x.Number.HasValue, () =>
            {
                RuleFor(x => x.Number)
                    .NotEmpty()
                    .Must((x) => x > 0)
                    .WithMessage(localizerMessages["MustBeGreaterThanZero"]);
            });
            When(x => x.SerieId.HasValue, () =>
            {
                RuleFor(x => x.SerieId)
                    .NotEmpty()
                    .Must((x) => x > 0)
                    .WithMessage(localizerMessages["MustBeGreaterThanZero"]);
            });
        }
    }
}
