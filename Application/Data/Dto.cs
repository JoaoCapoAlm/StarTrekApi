namespace Application.Data
{
    public record CreateNewSerieDto(string Abbreviation, string Imdb, string SynopsisResource, byte Timeline);

    public record CreateMovieDto(string OriginalName,
        string SynopsisResource,
        short OriginalLanguageId,
        DateOnly? ReleaseDate,
        short Time,
        string ImdbId,
        byte TimelineId,
        int TmdbId);
}
