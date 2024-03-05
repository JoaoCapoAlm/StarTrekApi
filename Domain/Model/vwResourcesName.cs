namespace Domain.Model
{
    public class vwResourcesName
    {
        public int Id { get; set; }
        public string Resource { get; set; }

        public vwResourcesName()
        {
        }

        public vwResourcesName(string resource)
        {
            Resource = resource;
        }
    }
}
