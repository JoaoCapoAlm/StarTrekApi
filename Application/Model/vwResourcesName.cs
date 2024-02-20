namespace Application.Model
{
    public class vwResourcesName
    {
        public string Id { get; set; }
        public string SynopsisResource { get; set; }
        public string TitleResource { get; set; }
        
        public vwResourcesName()
        {
        }

        public vwResourcesName(string synopsisResource, string titleResource)
        {
            SynopsisResource = synopsisResource;
            TitleResource = titleResource;
        }
    }
}
