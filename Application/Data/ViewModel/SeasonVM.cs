using System.ComponentModel;

namespace Application.Data.ViewModel
{
    [DisplayName("Season")]
    public class SeasonVM
    {
        public short ID { get; set; }
        public byte Number { get; set; }
        public ICollection<EpisodeVM> Episodes { get; set; }
    }
}
