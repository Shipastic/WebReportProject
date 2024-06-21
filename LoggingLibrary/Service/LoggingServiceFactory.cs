using Microsoft.Extensions.Logging;

namespace LoggingLibrary.Service
{
    public static class LoggingServiceFactory
    {
        public static ILoggingBuilder AddFile(this ILoggingBuilder builder, string filePath)
        {
            builder.AddProvider(new LogDirectoryManager(filePath));
            return builder;
        }
    }
}
