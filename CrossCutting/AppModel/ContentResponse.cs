namespace CrossCutting.AppModel
{
    public class ContentResponse
    {
        public string title { get; set; }
        public IDictionary<string, IEnumerable<string>> errors { get; set; }
    }
}
