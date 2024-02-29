using AutoMapper;
using CrossCutting.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Domain.Validation
{
    public class CreateSeasonWithSerieIdValidation : AbstractValidator<CreateSeasonWithSerieIdDto>
    {
        public CreateSeasonWithSerieIdValidation(IMapper mapper, IStringLocalizer<Messages> localizer, StarTrekContext context)
        {
            RuleFor(x => x.SerieId)
                .NotEmpty();

            RuleFor(x => mapper.Map<CreateSeasonDto>(x))
                .SetValidator(x => new CreateSeasonValidation(localizer, context));
        }
    }
}
