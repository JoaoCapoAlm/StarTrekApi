using Application.Resources;
using Domain;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Data.Validation
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
                    var qtdEpisodes = seasonDto.Episodes.Count();
                    var qtdResources = seasonDto.Episodes.Select(x => x.SynopsisResource)
                        .Distinct()
                        .Count();

                    if (qtdEpisodes > qtdResources)
                        return false;

                    qtdResources = seasonDto.Episodes.Select(x => x.TitleResource)
                        .Distinct()
                        .Count();

                    if (qtdEpisodes > qtdResources)
                        return false;

                    return true;
                }).WithMessage(localizer["SomeResourceIsDuplicated"]);
        }
    }
}
