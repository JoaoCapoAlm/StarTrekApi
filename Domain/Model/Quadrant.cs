namespace Domain.Model
{
    public class Quadrant
    {
        public short QuadrantId { get; set; }
        public string QuadrantResource { get; set; }
        public virtual ICollection<Place> Places { get; set; }
    }
}
