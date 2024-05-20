namespace Domain.DTOs
{
    public record CreateCharacterDto(
        string Name,
        short? YearBirth,
        byte? MonthBirth,
        byte? DayBirth,
        short? YearDeath,
        byte? MonthDeath,
        byte? DayDeath,
        short SpeciesId
    );
}
