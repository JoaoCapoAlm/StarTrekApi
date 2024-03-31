using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Model
{
    public class PlaceType
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte PlaceTypeId { get; set; }
        public string Type { get; set; }
        public virtual ICollection<Place> Places { get; set; }
    }
}
