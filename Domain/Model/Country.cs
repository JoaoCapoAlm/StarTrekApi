using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Model
{
    public class Country
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short CountryId { get; set; }
        public string ResourceName { get; set; }
        public virtual ICollection<Crew> Crews { get; set; }
    }
}
