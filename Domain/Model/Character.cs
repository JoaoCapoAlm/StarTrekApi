namespace Domain.Model
{
    public class Character : People
    {
        public int CharacterId { get; set; }
        public DateOnly? DateBirth { get; set; }
        public short SpeciesId { get; set; }
        public virtual Species Species { get; set; }
        public byte ClassificationId { get; set; }
        public virtual CharacterClassification CharacterClassification { get; set; }
    }
}
