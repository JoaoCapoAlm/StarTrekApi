using AutoMapper;
using CrossCutting.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Domain.Validation
{
    public class CreateSeasonWithSerieIdValidation : AbstractValidator<CreateSeasonWithSerieIdDto>
    {
        private readonly IStringLocalizer<Messages> _localizerMessages;

        public CreateSeasonWithSerieIdValidation(IMapper mapper,
            IStringLocalizer<Messages> localizerMessages,
            StarTrekContext context)
        {
            _localizerMessages = localizerMessages;

            RuleFor(x => x.SerieId)
                .NotEmpty()
                .WithMessage(_localizerMessages["MustBeGreaterThanZero"]);

            RuleFor(x => mapper.Map<CreateSeasonDto>(x))
                .SetValidator(x => new CreateSeasonValidation(_localizerMessages, context));
        }
    }
}
