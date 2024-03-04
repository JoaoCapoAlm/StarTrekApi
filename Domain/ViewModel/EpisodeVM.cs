namespace Application.Data.ViewModel
{
    public class EpisodeVM
    {
        public int ID { get; set; }
        public DateOnly? RealeaseDate { get; set; }
        public byte? Time { get; set; }
        public byte Number { get; set; }
        public float? StardateFrom { get; set; }
        public float? StardateTo { get; set; }
        public string ImdbId { get; set; }
        public string TitleTranslated { get; set; }
        public string SynopsisTranslated { get; set; }
    }
}
