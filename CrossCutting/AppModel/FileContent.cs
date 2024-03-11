namespace CrossCutting.AppModel
{
    public class FileContent
    {
        public required byte[] Content { get; set; }
        public required string ContentType { get; set; }
        public string? FileDownloadName { get; set; }
    }
}
