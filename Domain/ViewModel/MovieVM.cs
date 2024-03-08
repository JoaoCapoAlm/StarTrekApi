namespace Domain.ViewModel
{
    public class MovieVM
    {
        public short Id { get; set; }
        public string OriginalName { get; set; }
        public string TranslatedName { get; set; }
        public string Synopsis { get; set; }
        public string OriginalLanguage { get; set; }
        public DateOnly? ReleaseDate { get; set; }
        public short Time { get; set; }
        public TimelineVM Timeline { get; set; }
        public string ImdbId { get; set; }

        public MovieVM()
        {
        }
    }
}
