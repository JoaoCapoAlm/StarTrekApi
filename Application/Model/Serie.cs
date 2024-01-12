namespace Application.Model
{
    public class Serie
    {
        public short SerieId { get; set; }
        public string OriginalName { get; set; }
        public short OriginalLanguageId { get; set; }
        public virtual Language Language { get; set; }
        public short TimelineId { get; set; }
        public virtual Timeline Timeline { get; set; }
        public string ImdbId { get; set; }
        public string SynopsisResource { get; set; }
        public virtual ICollection<Season> Seasons { get; set; }
    }
}
