using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Model
{
    public class Place
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short PlaceId { get; set; }
        public string NameResource { get; set; }
        public byte QuadrantId { get; set; }
        public virtual Quadrant Quadrant { get; set; }
        public byte PlaceTypeId { get; set; }
        public virtual PlaceType PlaceType { get; set; }
        public virtual ICollection<Species> Species { get; set; }
    }
}
