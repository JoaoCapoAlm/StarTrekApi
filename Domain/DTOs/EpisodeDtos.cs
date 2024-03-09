namespace Domain.DTOs
{
    public record CreateEpisodeDto(
        DateOnly? RealeaseDate,
        string TitleResource,
        byte? Time,
        byte Number,
        float? StardateFrom,
        float? StardateTo,
        string ImdbId);

    public record CreateEpisodeWithSeasonIdDto(
        short SeasonId,
        DateOnly? RealeaseDate,
        string TitleResource,
        byte? Time,
        byte Number,
        float? StardateFrom,
        float? StardateTo,
        string ImdbId);

    public record UpdateEpisodeDto(
        DateOnly? RealeaseDate,
        byte? Time,
        byte? Number,
        float? StardateFrom,
        float? StardateTo,
        string ImdbId,
        short? SeasonId);
}
