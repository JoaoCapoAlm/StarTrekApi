using System.ComponentModel;

namespace Application.Data.ViewModel
{
    [DisplayName("Episodes")]
    public class EpisodeVM
    {
        public int ID { get; set; }
        public short SeasonId { get; set; }
        public DateOnly? RealeaseDate { get; set; }
        public byte? Time { get; set; }
        public byte Number { get; set; }
        public float? StardateFrom { get; set; }
        public float? StardateTo { get; set; }
        public string ImdbId { get; set; }
    }
}
