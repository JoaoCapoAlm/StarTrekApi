namespace Application.Model
{
    public class Movie
    {
        public short MovieId { get; set; }
        public string OriginalName { get; set; }
        public string Synopsis { get; set; }
        public short OriginalLanguageId { get; set; }
        public virtual Language Languages { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public short Time { get; set; }
        public string ImdbId { get; set; }

        public Movie() { }
    }
}
