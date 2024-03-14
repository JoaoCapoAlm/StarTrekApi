namespace Domain.Model
{
    public class CharacterClassification
    {
        public byte CharacterClassificationId { get; set; }
        public string Classification { get; set; }
        public virtual ICollection<Character> Characters { get; set; }
    }
}
