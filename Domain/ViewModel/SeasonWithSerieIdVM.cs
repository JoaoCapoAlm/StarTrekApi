using Domain.Model;

namespace Application.Data.ViewModel
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
