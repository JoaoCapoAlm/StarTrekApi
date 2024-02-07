using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Data.Validation
{
    public class CreateSeasonValidation : AbstractValidator<CreateSeasonDto>
    {
        public CreateSeasonValidation(IStringLocalizer<Messages> localizer)
        {
            RuleFor(s => s.Number)
                .NotEmpty()
                .WithMessage(localizer["MustBeGreaterThanZero"].Value);

            RuleForEach(s => s.Episodes)
                .SetValidator(new CreateEpisodeValidator(localizer));
        }
    }
}
