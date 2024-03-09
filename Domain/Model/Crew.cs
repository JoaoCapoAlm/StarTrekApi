namespace Domain.Model
{
    public class Crew
    {
        public int CrewId { get; set; }
        public string Name { get; set; }
        public DateOnly BirthDate { get; set; }
        public short? CountryId { get; set; }
        public virtual Country Country { get; set; }
        public DateOnly? DeathDate { get; set; }
        public ICollection<CrewRole> CrewRoles { get; set; }
    }
}
