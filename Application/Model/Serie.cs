using System;

namespace Application.Model
{
    public class Serie
    {
        public short SerieId { get; set; }
        public string OriginalName { get; set; }
        public short OriginalLanguageId { get; set; }
        public virtual Language Language { get; set; }
        public byte TimelineId { get; set; }
        public virtual Timeline Timeline { get; set; }
        public string ImdbId { get; set; }
        public string Abbreviation { get; set; }
        public string SynopsisResource { get; set; }
        public virtual ICollection<Season> Seasons { get; set; }
        public int TmdbId { get; set; }
        public DateTime? DateSyncTmdb { get; set; }
    }
}
