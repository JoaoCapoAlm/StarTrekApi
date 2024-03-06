namespace Domain.Model
{
    public class PlaceType
    {
        public byte PlaceTypeId { get; set; }
        public string Type { get; set; }
        public virtual ICollection<Place> Places { get; set; }
    }
}
