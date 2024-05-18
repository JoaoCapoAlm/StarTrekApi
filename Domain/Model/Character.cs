using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Model
{
    public class Character : People
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CharacterId { get; set; }
        public DateOnly? DateBirth { get; set; }
        public short SpeciesId { get; set; }
        public virtual Species Species { get; set; }
    }
}
