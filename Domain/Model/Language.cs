namespace Domain.Model
{
    public class Language
    {
        public short LanguageId { get; set; }
        public string ResourceName { get; set; }
        public string CodeISO { get; set; }
        public virtual ICollection<Movie> Movies { get; set; }
        public virtual ICollection<Serie> Series { get; set; }
    }
}
