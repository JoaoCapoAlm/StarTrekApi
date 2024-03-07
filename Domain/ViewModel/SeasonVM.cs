using System.Collections.ObjectModel;
using Domain.Model;

namespace Domain.ViewModel
{
    public class SeasonVM
    {
        public short ID { get; set; }
        public byte Number { get; set; }
        public ICollection<EpisodeVM> Episodes { get; set; }
        public SeasonVM()
        {
            Episodes = [];
        }
        public SeasonVM(short id, byte number, IEnumerable<Episode> episodes)
        {
            ID = id;
            Number = number;
            Episodes = [];
            foreach (var episode in episodes)
            {
                Episodes.Add(new EpisodeVM
                {
                    ID = episode.EpisodeId,
                    ImdbId = episode.ImdbId,
                    Number = episode.Number,
                    RealeaseDate = episode.RealeaseDate,
                    StardateFrom = episode.StardateFrom,
                    StardateTo = episode.StardateTo,
                    Time = episode.Time,
                    Synopsis = episode.SynopsisResource,
                    TranslatedTitle = episode.TitleResource
                });
            };
        }
    }
}
