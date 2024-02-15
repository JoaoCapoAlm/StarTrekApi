namespace Application.Data.ViewModel
{
    public class SerieVM
    {
        public short ID { get; set; }
        public string Abbreviation { get; set; }
        public string ImdbId { get; set; }
        public string OriginalLanguage { get; set; }
        public string OriginalName { get; set; }
        public string NameTranslated { get; set; }
        public string Synopsis { get; set; }
        public byte Timeline { get; set; }
        public ICollection<SeasonVM> Seasons { get; set; }
    }
}
