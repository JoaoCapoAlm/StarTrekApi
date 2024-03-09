using CrossCutting.Enums;

namespace Domain.DTOs
{
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
}
