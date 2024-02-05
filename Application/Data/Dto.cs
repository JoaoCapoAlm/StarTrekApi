using Application.Data.Enum;

namespace Application.Data
{
    public record CreateNewSerieDto(string Abbreviation, string Imdb, string SynopsisResource, byte Timeline);

    public record CreateMovieDto(string OriginalName,
        string SynopsisResource,
        string OriginalLanguageIso,
        DateOnly? ReleaseDate,
        short Time,
        string ImdbId,
        TimelineEnum TimelineId,
        int TmdbId);

    public record UpdateMovieDto(string OriginalName,
        string SynopsisResource,
        string OriginalLanguageIso,
        DateOnly? ReleaseDate,
        short? Time,
        string ImdbId,
        byte? TimelineId,
        int? TmdbId);
}
