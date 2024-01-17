namespace Application.Model
{
    public class Crew
    {
        public int CrewId { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public short? CountryId { get; set; }
        public virtual Country Country { get; set; }
        public DateTime? DeathDate { get; set; }
        public ICollection<CrewRole> CrewRoles { get; set; }
    }
}
