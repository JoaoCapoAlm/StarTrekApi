namespace Domain.ViewModel
{
    public class SerieVM
    {
        public short ID { get; set; }
        public string Abbreviation { get; set; }
        public string ImdbId { get; set; }
        public LanguageVM OriginalLanguage { get; set; }
        public string OriginalName { get; set; }
        public string TranslatedName { get; set; }
        public string Synopsis { get; set; }
        public TimelineVM Timeline { get; set; }
        public ICollection<SeasonVM> Seasons { get; set; }
    }
}
