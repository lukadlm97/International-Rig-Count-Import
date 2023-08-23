using Microsoft.Extensions.Logging;

namespace Homework.Enverus.Shared.Logging.Contracts
{
    public interface IHighPerformanceLogger
    {
        void Log(string message, LogLevel logLevel);
        void Log(string message, Exception innerException, LogLevel logLevel);
    }
}
