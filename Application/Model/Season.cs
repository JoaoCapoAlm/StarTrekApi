namespace Application.Model
{
    public class Season
    {
        public short SeasonId { get; set; }
        public short SerieId { get; set; }
        public virtual Serie Serie { get; set; }
        public byte Number { get; set; }
        public virtual ICollection<Episode> Episodes { get; set; }
    }
}
