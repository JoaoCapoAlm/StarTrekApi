using Domain.Model;

namespace Domain.ViewModel
{
    public class SeasonWithSerieIdVM : SeasonVM
    {
        public short SerieId { get; set; }

        public SeasonWithSerieIdVM(short id, short serieId, byte number, IEnumerable<Episode> episodes)
            : base(id, number, episodes)
        {
            SerieId = serieId;
        }
    }
}
