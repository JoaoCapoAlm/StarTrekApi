namespace Application.Data.ViewModel
{
    public class TimelineVM
    {
        public byte ID { get; set; }
        public string Name { get; set; }
        public TimelineVM()
        {
        }
        public TimelineVM(byte id, string name)
        {
            ID = id;
            Name = name;
        }
    }
}
