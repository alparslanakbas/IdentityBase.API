[Route("api/[controller]/[action]")]
[ApiController]
public class LogsController : ControllerBase
{
    private readonly ILogRepository _logRepository;

    public LogsController(ILogRepository logRepository)
    {
        _logRepository = logRepository;
    }

    [HttpGet]
    public ActionResult<IEnumerable<LogFileInfo>> GetLogFiles()
    {
        var files = _logRepository.GetLogFiles();
        if (!files.Any())
            return NotFound("Log dosyası bulunamadı");

        return Ok(files);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LogEntry>>> GetInfoLogs()
    {
        var logs = await _logRepository.GetInfoLogs();
        if (!logs.Any())
            return NotFound("Info log içeriği bulunamadı");

        return Ok(logs);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LogEntry>>> GetErrorLogs()
    {
        var logs = await _logRepository.GetErrorLogs();
        if (!logs.Any())
            return NotFound("Error log içeriği bulunamadı");

        return Ok(logs);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LogEntry>>> GetRequestLogs()
    {
        var logs = await _logRepository.GetRequestLogs();
        if (!logs.Any())
            return NotFound("Request log içeriği bulunamadı");

        return Ok(logs);
    }
}