using Homework.Enverus.Shared.Logging.Contracts;
using Homework.Enverus.Shared.Logging.Utilities;
using Microsoft.Extensions.Logging;

namespace Homework.Enverus.Shared.Logging.Implementations
{
    public class HighPerformanceLogger : IHighPerformanceLogger
    {
        private readonly ILogger<HighPerformanceLogger> _logger;

        public HighPerformanceLogger(ILogger<HighPerformanceLogger> logger)
        {
            _logger = logger;
        }

        public void Log(string message, LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Information:
                    LogProvider.InformationalMessage(_logger, message);
                    break;
                case LogLevel.Trace:
                    LogProvider.TraceMessage(_logger, message);
                    break;
                case LogLevel.Error:
                    LogProvider.ErrorMessage(_logger, message);
                    break;
                case LogLevel.Warning:
                    LogProvider.WarningMessage(_logger, message);
                    break;
                case LogLevel.Debug:
                    LogProvider.DebugMessage(_logger, message);
                    break;
            }
        }

        public void Log(string message, Exception innerException, LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Information:
                    LogProvider.InformationalMessage(_logger, message, innerException);
                    break;
                case LogLevel.Trace:
                    LogProvider.TraceMessage(_logger, message, innerException);
                    break;
                case LogLevel.Error:
                    LogProvider.ErrorMessage(_logger, message, innerException);
                    break;
                case LogLevel.Warning:
                    LogProvider.WarningMessage(_logger, message, innerException);
                    break;
                case LogLevel.Debug:
                    LogProvider.DebugMessage(_logger, message, innerException);
                    break;
            }
        }
    }
}
