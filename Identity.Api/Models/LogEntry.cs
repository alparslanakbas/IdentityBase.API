namespace Identity.Api.Models
{
    public class LogEntry
    {
        [JsonPropertyName("time")]
        public DateTime Time { get; set; }
        [JsonPropertyName("level")]
        public string? Level { get; set; }
        [JsonPropertyName("message")]
        public string? Message { get; set; }
    }
}
