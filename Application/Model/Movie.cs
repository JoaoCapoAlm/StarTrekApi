namespace Application.Model
{
    public class Movie
    {
        public int MovieId { get; set; }
        public string OriginalName { get; set; }
        public string Synopsis { get; set; }
        public string OriginalLanguage { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public short Time { get; set; }
        public short ImdbId { get; set; }

        public Movie() { }
    }
}
