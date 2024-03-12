namespace Domain.Model
{
    public class Movie
    {
        public short MovieId { get; set; }
        public string OriginalName { get; set; }
        public string TitleResource { get; set; }
        public string SynopsisResource { get; set; }
        public short OriginalLanguageId { get; set; }
        public virtual Language Languages { get; set; }
        public DateOnly? ReleaseDate { get; set; }
        public short Time { get; set; }
        public string ImdbId { get; set; }
        public byte TimelineId { get; set; }
        public virtual Timeline Timeline { get; set; }
        public int TmdbId { get; set; }
        public DateTime? DateSyncTmdb { get; set; }

        public Movie() { }
    }
}
