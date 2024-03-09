namespace Domain.DTOs
{
    public record CreateSeasonDto(byte Number, IList<CreateEpisodeDto> Episodes);

    public record CreateSeasonWithSerieIdDto(byte SerieId, byte Number, IList<CreateEpisodeDto> Episodes);

    public record UpdateSeasonDto(byte? SerieId, byte? Number);
}
