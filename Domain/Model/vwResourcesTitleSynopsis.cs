namespace Domain.Model
{
    public class vwResourcesTitleSynopsis
    {
        public int Id { get; set; }
        public string Resource { get; set; }

        public vwResourcesTitleSynopsis()
        {
        }

        public vwResourcesTitleSynopsis(string resource)
        {
            Resource = resource;
        }
    }
}
