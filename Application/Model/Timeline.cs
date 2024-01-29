﻿namespace Application.Model
{
    public class Timeline
    {
        public byte TimelineId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Movie> Movies { get; set; }
        public virtual ICollection<Serie> Series { get; set; }
    }
}
