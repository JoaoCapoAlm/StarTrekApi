using CrossCutting.Enums;
using CrossCutting.Helpers;
using CrossCutting.Resources;
using Domain.Repositories;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Domain.Validation
{
    public class UpdateMovieValidation : AbstractValidator<UpdateMovieDto>
    {
        public UpdateMovieValidation(IStringLocalizer<Messages> localizer, StarTrekContext context)
        {
            var viewsRepository = new ViewsRepository(context);

            RuleFor(m => m.Time)
                .GreaterThan((short)0)
                .WithMessage(localizer["ValueGreaterThanZero"].Value);

            When(m => !string.IsNullOrWhiteSpace(m.ImdbId), () =>
            {
                RuleFor(m => m.ImdbId)
                    .Length(8, 14)
                        .WithMessage(localizer["Invalid"].Value)
                    .Must(m => m.StartsWith("tt") && RegexHelper.StringIsNumeric(m[2..]))
                        .WithMessage(localizer["Invalid"].Value);
            });

            RuleFor(m => m.TimelineId)
                .IsInEnum()
                .WithMessage(localizer["Invalid"].Value);

            When(x => !string.IsNullOrEmpty(x.TitleResource), () =>
            {
                RuleFor(m => m.TitleResource)
                    .Cascade(CascadeMode.Stop)
                    .Must(RegexHelper.StringIsSimpleAlphabetOrNumber)
                        .WithMessage($"{localizer["ShouldBeLettersWithoutAccentsOrNumbers"].Value}")
                    .Must(s => !s.EndsWith("Synopsis", StringComparison.CurrentCultureIgnoreCase))
                        .WithMessage(localizer["MustNotContainSynopsisAtTheEnd"].Value)
                    .MustAsync(async (resource, cancellationToken) =>
                        {
                            var checkExist = await viewsRepository.CheckResourceExists(resource, cancellationToken: cancellationToken);
                            return !checkExist;
                        }).WithMessage(localizer["AlreadyExists"]);
            });

            When(x => x.ReleaseDate.HasValue, () =>
            {
                RuleFor(m => m.ReleaseDate)
                    .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
                    .WithMessage(localizer["Invalid"].Value);
            });

            When(m => !string.IsNullOrEmpty(m.SynopsisResource), () =>
            {
                RuleFor(m => m.SynopsisResource)
                    .Cascade(CascadeMode.Stop)
                    .Must(RegexHelper.StringIsSimpleAlphabetOrNumber)
                        .WithMessage($"{localizer["ShouldBeLettersWithoutAccentsOrNumbers"].Value}")
                    .Must(s => s.EndsWith("Synopsis", StringComparison.CurrentCultureIgnoreCase))
                        .WithMessage(localizer["MustContainSynopsisAtTheEnd"])
                    .MustAsync(async (resource, cancellationToken) =>
                    {
                        var checkExist = await viewsRepository.CheckResourceExists(resource, cancellationToken: cancellationToken);
                        return !checkExist;
                    }).WithMessage(localizer["AlreadyExists"]);
            });

            When(m => !string.IsNullOrWhiteSpace(m.OriginalLanguageIso), () =>
            {
                RuleFor(x => x.OriginalLanguageIso)
                    .Must((dto, language) =>
                    {
                        var languageIso = RegexHelper.RemoveSpecialCharacters(language);
                        return Enum.IsDefined(typeof(LanguageEnum), languageIso);
                    }).WithMessage(localizer["LanguageCodeMustIso"].Value);
            });
        }
    }
}
