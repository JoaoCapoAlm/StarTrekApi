using CrossCutting.Resources;
using Domain.DTOs;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Domain.Validation
{
    public class CreateSeasonValidation : AbstractValidator<CreateSeasonDto>
    {
        public CreateSeasonValidation(IStringLocalizer<Messages> localizer, StarTrekContext context)
        {
            RuleFor(s => s.Number)
                .NotEmpty()
                .WithMessage(localizer["MustBeGreaterThanZero"].Value);

            RuleForEach(s => s.Episodes)
                .SetValidator(new CreateEpisodeValidator(localizer, context))
                .Must((seasonDto, episodeDto) =>
                {
                    return seasonDto.Episodes
                        .Where(x => x.TitleResource.Equals(episodeDto.TitleResource))
                        .Count() == 1;
                }).WithMessage(localizer["DuplicateResource"])
                .Must((seasonDto, episodeDto) =>
                {
                    return seasonDto.Episodes
                        .Where(x => x.Number.Equals(episodeDto.Number))
                        .Count() == 1;
                }).WithMessage(localizer["DuplicateEpisodeNumber"]);
        }
    }
}
