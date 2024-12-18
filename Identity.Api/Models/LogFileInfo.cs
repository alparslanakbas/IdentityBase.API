namespace Identity.Api.Models
{
    public class LogFileInfo
    {
        public string FileName { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public DateTime LastModified { get; set; } = DateTime.Now;
    }
}
