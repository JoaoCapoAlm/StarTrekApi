using CrossCutting.Enums;

namespace Domain.DTOs
{
    public record UpdateSerieDto(
        string Abbreviation,
        string ImdbId,
        string OriginalLanguageIso,
        string OriginalName,
        TimelineEnum? TimelineId,
        int? TmdbId);

    public record CreateSerieDto(
        string Abbreviation,
        string ImdbId,
        string OriginalLanguageIso,
        string OriginalName,
        string TitleResource,
        string SynopsisResource,
        TimelineEnum TimelineId,
        int TmdbId,
        IList<CreateSeasonDto> Seasons);

    public record CreateNewSerieByTmdbDto(
        string Abbreviation,
        string Imdb,
        string TitleResource,
        string SynopsisResource,
        byte Timeline);
}
