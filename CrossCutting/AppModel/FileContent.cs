namespace CrossCutting.AppModel
{
    public class FileContent
    {
        public FileContent(byte[] content, string contentType, string? fileDownloadName)
        {
            Content = content;
            ContentType = contentType;
            FileDownloadName = fileDownloadName;
        }

        public byte[] Content { get; set; }
        public string ContentType { get; set; }
        public string? FileDownloadName { get; set; }
    }
}
