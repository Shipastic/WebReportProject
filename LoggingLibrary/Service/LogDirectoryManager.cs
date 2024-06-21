using Microsoft.Extensions.Logging;
namespace LoggingLibrary.Service
{
    public class LogDirectoryManager : ILoggerProvider
    {
        string path;
        public LogDirectoryManager(string path)
        {
            this.path = path;
        }
        public ILogger CreateLogger(string categoryName)
        {
            return new LoggingService(path);
        }
        public void Dispose() { }
    }
}
