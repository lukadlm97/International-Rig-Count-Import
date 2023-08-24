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
            services.AddLogging(
                builder =>
                {
                    builder.SetMinimumLevel(LogLevel.Trace);
                    builder.AddNLog(new NLogProviderOptions { RemoveLoggerFactoryFilter = true });
                });

            services.AddSingleton<IHighPerformanceLogger, HighPerformanceLogger>();
            return services;
        }

        public static IServiceCollection ConfigureLoggingServices(this IServiceCollection services)
        {
            services.AddLogging(
                builder =>
                {
                    builder.SetMinimumLevel(LogLevel.Trace);
                    builder.AddNLog(new NLogProviderOptions { RemoveLoggerFactoryFilter = true });
                });

            services.AddSingleton<IHighPerformanceLogger, HighPerformanceLogger>();
            return services;
        }

        public static ILoggingBuilder ConfigureLoggingBuilder(this ILoggingBuilder loggingBuilder)
        {
            loggingBuilder.ClearProviders();
            return loggingBuilder;
        }
        
    }
}
