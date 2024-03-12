namespace Domain.Model
{
    public class Place
    {
        public short PlaceId { get; set; }
        public string NameResource { get; set; }
        public byte QuadrantId { get; set; }
        public Quadrant Quadrant { get; set; }
        public byte PlaceTypeId { get; set; }
        public PlaceType PlaceType { get; set; }
        public virtual ICollection<Species> Species { get; set; }
    }
}
