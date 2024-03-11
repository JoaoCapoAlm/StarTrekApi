using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Domain.Model
{
    public class vwFullSerie
    {
        public short SerieId { get; set; }
        public string OriginalName { get; set; }
        public string SerieTitleResource { get; set; }
        public string SerieSynopsisResource { get; set; }
        public string Abbreviation { get; set; }
        public string ImdbSerie { get; set; }
        public byte TimelineId { get; set; }
        public string TimelineName { get; set; }
        public short LanguageId { get; set; }
        public string CodeISO { get; set; }
        public string LanguageResourceName { get; set; }
        public short SeasonId { get; set; }
        public byte SeasonNumber { get; set; }
        public int EpisodeId { get; set; }
        public byte EpisodeNumber { get; set; }
        public DateOnly? RealeaseDate { get; set; }
        public float StardateFrom { get; set; }
        public float StardateTo { get; set; }
        public string EpisodeSynopsisResource { get; set; }
        public string EpisodeTitleResource { get; set; }
        public byte? Time { get; set; }
        public string ImdbEpisode { get; set; }
    }
}
