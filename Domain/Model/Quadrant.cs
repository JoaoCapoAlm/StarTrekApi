using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Model
{
    public class Quadrant
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte QuadrantId { get; set; }
        public string QuadrantResource { get; set; }
        public virtual ICollection<Place> Places { get; set; }
    }
}
