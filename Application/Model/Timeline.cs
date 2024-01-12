namespace Application.Model
{
    public class Timeline
    {
        public short TimelineId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Movie> Movies { get; set; }
        public virtual ICollection<Serie> Series { get; set; }
    }
}
