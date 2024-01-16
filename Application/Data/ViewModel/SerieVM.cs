namespace Application.Data.ViewModel
{
    
    public class SerieVM
    {
        public short ID { get; set; }
        public string OriginalName { get; set; }
        public string Language { get; set; }
        public string ImdbId { get; set; }
        public string Abbreviation { get; set; }
        public TimelineVM Timeline { get; set; }
        public ICollection<SeasonVM> Seasons { get; set; }
    }
}
