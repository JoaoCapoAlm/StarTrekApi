namespace Domain.ViewModel
{
    public class PlaceVM
    {
        public short ID { get; set; }
        public string Name { get; set; }
        public QuadrantVM Quadrant { get; set; }
        public PlaceTypeVM PlaceType { get; set; }
    }
}
