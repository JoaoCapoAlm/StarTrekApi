namespace Domain.Model
{
    public class Quadrant
    {
        public byte QuadrantId { get; set; }
        public string QuadrantResource { get; set; }
        public virtual ICollection<Place> Places { get; set; }
    }
}
