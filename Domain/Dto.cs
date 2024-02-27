using CrossCutting.Enums;

namespace Domain
{
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

    public record CreateNewSerieByTmdbDto(string Abbreviation, string Imdb, string TitleResource, string SynopsisResource, byte Timeline);

    public record CreateEpisodeDto(
        DateOnly? RealeaseDate,
        string TitleResource,
        string SynopsisResource,
        byte? Time,
        byte Number,
        float? StardateFrom,
        float? StardateTo,
        string ImdbId);

    public record CreateMovieDto(string OriginalName,
        string TitleResource,
        string SynopsisResource,
        string OriginalLanguageIso,
        DateOnly? ReleaseDate,
        short Time,
        string ImdbId,
        TimelineEnum TimelineId,
        int TmdbId);

    public record UpdateMovieDto(string OriginalName,
        string TitleResource,
        string SynopsisResource,
        string OriginalLanguageIso,
        DateOnly? ReleaseDate,
        short? Time,
        string ImdbId,
        TimelineEnum? TimelineId,
        int? TmdbId);

    public record CreateSeasonDto(byte Number, IList<CreateEpisodeDto> Episodes);

    public record UpdateSerieDto(
        string Abbreviation,
        string ImdbId,
        string OriginalLanguageIso,
        string OriginalName,
        TimelineEnum? TimelineId,
        int? TmdbId);
}
