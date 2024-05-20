using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Model
{
    public class Character
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CharacterId { get; set; }
        public string Name { get; set; }
        public short? YearBirth { get; set; }
        public byte? MonthBirth { get; set; }
        public byte? DayBirth { get; set; }
        public short? YearDeath { get; set; }
        public byte? MonthDeath { get; set; }
        public byte? DayDeath { get; set; }
        public short SpeciesId { get; set; }
        public virtual Species Species { get; set; }
    }
}
