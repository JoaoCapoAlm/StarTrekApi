using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Model
{
    public class Timeline
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte TimelineId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Movie> Movies { get; set; }
        public virtual ICollection<Serie> Series { get; set; }
    }
}
