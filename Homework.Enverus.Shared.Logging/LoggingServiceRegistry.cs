using Homework.Enverus.Shared.Logging.Contracts;
using Homework.Enverus.Shared.Logging.Implementations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace Homework.Enverus.Shared.Logging
{
    public static class LoggingServiceRegistry
    {
        public static IServiceCollection ConfigureLoggingServices(this IServiceCollection services, ILoggingBuilder loggingBuilder)
        {
            loggingBuilder.ClearProviders();
           // loggingBuilder.AddConsole();
            services.AddLogging(
                builder =>
                {
                   // builder.AddConsole().SetMinimumLevel(LogLevel.Trace);
                    builder.SetMinimumLevel(LogLevel.Trace);
                    builder.AddNLog(new NLogProviderOptions { RemoveLoggerFactoryFilter = true });
                });

            services.AddSingleton<IHighPerformanceLogger, HighPerformanceLogger>();
            return services;
        }
    }
}
