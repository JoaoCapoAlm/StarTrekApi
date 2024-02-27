using Application.Data.Enums;
using CrossCutting.Helpers;
using Application.Repositories;
using Application.Resources;
using Domain;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Data.Validation
{
    public class CreateSerieValidation : AbstractValidator<CreateSerieDto>
    {
        public CreateSerieValidation(IStringLocalizer<Messages> localizer, StarTrekContext context)
        {
            var viewsRepository = new ViewsRepository(context);
            var serieRepository = new SerieRepository(context);

            RuleFor(s => s.Abbreviation)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                    .WithMessage(localizer["Required"].Value)
                .Length(3)
                    .WithMessage(localizer["Invalid"].Value);

            When(x => !string.IsNullOrWhiteSpace(x.ImdbId), () =>
            {
                RuleFor(m => m.ImdbId)
                    .Cascade(CascadeMode.Stop)
                    .MinimumLength(9)
                        .WithMessage(localizer["InvalidLength"])
                    .Must(m => m.StartsWith("tt") && RegexHelper.StringIsNumeric(m[2..]))
                        .WithMessage(localizer["Invalid"].Value)
                    .MustAsync(async (imdb, cancellationToken) =>
                    {
                        var checkExists = await serieRepository.CheckImdbOrTmdbExists(imdb, null, cancellationToken);
                        return checkExists.Equals(false);
                    }).WithMessage(localizer["AlreadyExists"]);
            });

            RuleFor(s => s.OriginalLanguageIso)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                    .WithMessage(localizer["Required"].Value)
                .Must((dto, language) =>
                {
                    var languageIso = RegexHelper.RemoveSpecialCharacters(language);
                    return Enum.IsDefined(typeof(LanguageEnum), languageIso);
                }).WithMessage(localizer["LanguageCodeMustIso"].Value);

            RuleFor(s => s.OriginalName)
                .NotEmpty()
                .WithMessage(localizer["Required"].Value);

            RuleForEach(s => s.Seasons)
                .SetValidator(new CreateSeasonValidation(localizer, context));

            RuleFor(s => s.SynopsisResource)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                    .WithMessage(localizer["Required"].Value)
                .Must(RegexHelper.StringIsSimpleAlphabetOrNumber)
                    .WithMessage(localizer["ShouldBeLettersWithoutAccentsOrNumbers"].Value)
                .Must(r => r.EndsWith("Synopsis"))
                    .WithMessage(localizer["MustContainSynopsisAtTheEnd"])
                .MustAsync(async (resource, cancellation) =>
                {
                    var checkExists = await viewsRepository.CheckResourceExists(resource, cancellationToken: cancellation);
                    return checkExists.Equals(false);
                }).WithMessage(localizer["AlreadyExists"]);

            RuleFor(s => s.TimelineId)
                .IsInEnum()
                .WithMessage(localizer["Invalid"].Value);

            RuleFor(s => s.TmdbId)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(100).WithMessage(localizer["Invalid"].Value)
                .MustAsync(async (tmdb, cancellationToken) =>
                {
                    return false == await serieRepository.CheckImdbOrTmdbExists(string.Empty, tmdb, cancellationToken);
                }).WithMessage(localizer["AlreadyExists"]);

            RuleFor(s => s.TitleResource)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                    .WithMessage(localizer["Required"].Value)
                .Must(RegexHelper.StringIsSimpleAlphabetOrNumber)
                    .WithMessage(localizer["ShouldBeLettersWithoutAccentsOrNumbers"].Value)
                .Must(r => !r.EndsWith("Synopsis"))
                    .WithMessage(localizer["MustNotContainSynopsisAtTheEnd"])
                .MustAsync(async (resource, cancellation) =>
                {
                    var checkExists = await viewsRepository.CheckResourceExists(resource, cancellationToken: cancellation);
                    return checkExists.Equals(false);
                }).WithMessage(localizer["AlreadyExists"]);
        }
    }
}
