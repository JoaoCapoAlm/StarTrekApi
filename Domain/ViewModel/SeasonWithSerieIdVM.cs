using Domain.Model;

namespace Application.Data.ViewModel
{
    public class SeasonWithSerieIdVM
    {
        public short ID { get; set; }
        public short SerieId { get; set; }
        public byte Number { get; set; }
        public ICollection<EpisodeVM> Episodes { get; set; }
        public SeasonWithSerieIdVM()
        {
        }
        public SeasonWithSerieIdVM(short id, short serieId, byte number, IEnumerable<Episode> episodes)
        {
            ID = id;
            SerieId = serieId;
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
                    Time = episode.Time
                });
            };
        }
    }
}
