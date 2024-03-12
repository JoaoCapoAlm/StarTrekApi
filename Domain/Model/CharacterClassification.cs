namespace Domain.Model
{
    public class CharacterClassification
    {
        public short CharacterClassificationId { get; set; }
        public string Classification { get; set; }
        public virtual ICollection<Character> Characters { get; set; }
    }
}
