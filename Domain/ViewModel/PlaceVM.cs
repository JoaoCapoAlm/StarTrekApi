namespace Domain.ViewModel
{
    public class PlaceVM
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public byte QuadrantId { get; set; }
        public PlaceTypeVM PlaceType { get; set; }
    }
}
