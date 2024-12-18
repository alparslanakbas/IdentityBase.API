public class LogRepository : ILogRepository
{
    private readonly string _logPath;
    private static readonly StringComparer PathComparer = StringComparer.OrdinalIgnoreCase;
    private const string LOG_FOLDER = "logs";
    private const string LOG_EXTENSION = "*.log";
    private const string INFO_FOLDER = "info";
    private const string ERROR_FOLDER = "error";
    private const string REQUEST_FOLDER = "requests";

    public LogRepository()
    {
        var baseDir = AppDomain.CurrentDomain.BaseDirectory;
        _logPath = PathComparer.Equals(Path.GetFileName(baseDir), LOG_FOLDER)
            ? baseDir
            : Path.Combine(baseDir, LOG_FOLDER);
    }

    public IEnumerable<LogFileInfo> GetLogFiles()
    {
        if (!Directory.Exists(_logPath))
            return Enumerable.Empty<LogFileInfo>();

        var allFolders = new[] { INFO_FOLDER, ERROR_FOLDER, REQUEST_FOLDER };
        var result = new List<LogFileInfo>();

        foreach (var folder in allFolders)
        {
            var folderPath = Path.Combine(_logPath, folder);
            if (Directory.Exists(folderPath))
            {
                var files = Directory.GetFiles(folderPath, LOG_EXTENSION)
                    .Select(f => new LogFileInfo
                    {
                        FileName = Path.GetFileName(f),
                        Path = f,
                        LastModified = File.GetLastWriteTime(f)
                    });
                result.AddRange(files);
            }
        }

        return result.OrderByDescending(f => f.LastModified);
    }

    public async Task<IEnumerable<LogEntry>> GetInfoLogs()
    {
        var infoPath = Path.Combine(_logPath, INFO_FOLDER);
        var files = Directory.GetFiles(infoPath, LOG_EXTENSION).OrderByDescending(f => f);
        return files.Any() ? await ReadLogFileAsync(files.First()) : Enumerable.Empty<LogEntry>();
    }

    public async Task<IEnumerable<LogEntry>> GetErrorLogs()
    {
        var errorPath = Path.Combine(_logPath, ERROR_FOLDER);
        var files = Directory.GetFiles(errorPath, LOG_EXTENSION).OrderByDescending(f => f);
        return files.Any() ? await ReadLogFileAsync(files.First()) : Enumerable.Empty<LogEntry>();
    }

    public async Task<IEnumerable<LogEntry>> GetRequestLogs()
    {
        var requestPath = Path.Combine(_logPath, REQUEST_FOLDER);
        var files = Directory.GetFiles(requestPath, LOG_EXTENSION).OrderByDescending(f => f);
        return files.Any() ? await ReadLogFileAsync(files.First()) : Enumerable.Empty<LogEntry>();
    }

    private async Task<IEnumerable<LogEntry>> ReadLogFileAsync(string filePath)
    {
        if (!File.Exists(filePath))
            return Enumerable.Empty<LogEntry>();

        var entries = new List<LogEntry>();
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new DateTimeJsonConverter() }
        };

        using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using var reader = new StreamReader(fileStream);

        string? line;
        while ((line = await reader.ReadLineAsync()) is not null)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            try
            {
                var logEntry = JsonSerializer.Deserialize<LogEntry>(line, options);
                if (logEntry is not null)
                {
                    // Message içinde JSON var mı kontrol et
                    if (logEntry.Message?.StartsWith("{") == true && logEntry.Message.EndsWith("}"))
                    {
                        try
                        {
                            using JsonDocument doc = JsonDocument.Parse(logEntry.Message);
                            var root = doc.RootElement;

                            // Level bilgisini güncelle
                            if (root.TryGetProperty("level", out JsonElement levelElement))
                            {
                                logEntry.Level = levelElement.GetString();
                            }
                        }
                        catch
                        {
                               
                        }
                    }
                    entries.Add(logEntry);
                }
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"JSON format hatası: {ex.Message}");
                entries.Add(new LogEntry
                {
                    Time = DateTime.UtcNow,
                    Level = "UNKNOWN",
                    Message = line
                });
            }
        }

        return entries.OrderByDescending(e => e.Time);
    }

    private class DateTimeJsonConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            if (string.IsNullOrEmpty(value))
                return DateTime.UtcNow;

            // ISO 8601 formatını dene
            if (DateTime.TryParse(value, CultureInfo.InvariantCulture,
                DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal, out DateTime result))
            {
                return result;
            }

            // Özel formatları dene
            string[] formats = new[]
            {
            "yyyy-MM-dd HH:mm:ss.ffff",
            "yyyy-MM-ddTHH:mm:ss.fffffffZ",
            "yyyy-MM-dd HH:mm:ss",
            "yyyy-MM-ddTHH:mm:ss"
        };

            if (DateTime.TryParseExact(value, formats, CultureInfo.InvariantCulture,
                DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal, out DateTime exactResult))
            {
                return exactResult;
            }

            return DateTime.UtcNow;
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("o"));
        }
    }
}