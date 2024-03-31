using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Model
{
    public class CharacterClassification
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte CharacterClassificationId { get; set; }
        public string Classification { get; set; }
        public virtual ICollection<Character> Characters { get; set; }
    }
}
