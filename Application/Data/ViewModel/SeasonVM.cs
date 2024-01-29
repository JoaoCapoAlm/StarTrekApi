using Application.Model;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Application.Data.ViewModel
{
    public class SeasonVM {
        public short ID { get; set; }
        public byte Number { get; set; }
        public ICollection<EpisodeVM> Episodes { get; set; }
        public SeasonVM(short id, byte number, IEnumerable<Episode> episodes)
        {
            ID = id;
            Number = number;
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
                    SeasonId = episode.SeasonId
                });
            };
        }
    }
}
