namespace Domain.Model
{
    public class Species
    {
        public short SpeciesId { get; set; }
        public string Name { get; set; }
        public short PlanetId { get; set; }
        public Place Planet { get; set; }
    }
}
