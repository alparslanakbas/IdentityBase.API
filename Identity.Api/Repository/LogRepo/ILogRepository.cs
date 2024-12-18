namespace Identity.Api.Repository.LogRepo
{
    public interface ILogRepository
    {
        IEnumerable<LogFileInfo> GetLogFiles();
        Task<IEnumerable<LogEntry>> GetInfoLogs();
        Task<IEnumerable<LogEntry>> GetErrorLogs();
        Task<IEnumerable<LogEntry>> GetRequestLogs();
    }
}
