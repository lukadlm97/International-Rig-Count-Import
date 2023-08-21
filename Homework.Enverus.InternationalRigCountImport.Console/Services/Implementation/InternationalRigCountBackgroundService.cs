using Homework.Enverus.InternationalRigCountImport.Core.Fetchers.Contracts;
using Homework.Enverus.InternationalRigCountImport.Core.Models.Enums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Homework.Enverus.InternationalRigCountImport.Console.Services.Implementation
{
    public class InternationalRigCountBackgroundService : BackgroundService
    {
        private readonly ILogger<InternationalRigCountBackgroundService> _logger;

        public InternationalRigCountBackgroundService(IServiceProvider services,
            ILogger<InternationalRigCountBackgroundService> logger)
        {
            Services = services;
            _logger = logger;
        }

        public IServiceProvider Services { get; }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("International Rig Count Hosted Service running.");

            await DoWork(stoppingToken);
        }

        private async Task DoWork(CancellationToken stoppingToken)
        {
            _logger.LogInformation("International Rig Count Hosted Service is working.");

            using (var scope = Services.CreateScope())
            {
                var scopedProcessingService =
                    scope.ServiceProvider
                        .GetRequiredService<IWebPageProcessor>();

                var result = await scopedProcessingService.GetInternationalRigCount(stoppingToken);

                switch (result.Status)
                {
                    case OperationStatus.Ok:
                        if (_logger.IsEnabled(LogLevel.Information))
                        {
                            _logger.LogInformation("Successfully downloaded!!!");
                        }
                        await SaveFile(result.Result.FileBytes);
                        break;
                    case OperationStatus.BadRequest:
                    case OperationStatus.NotFound:
                    case OperationStatus.UnavailableAction:
                    case OperationStatus.Unknown:
                        if (_logger.IsEnabled(LogLevel.Error))
                        {
                            _logger.LogError("Operation completed with status error: "+result.Description);
                        }
                        break;
                    default:
                        if (_logger.IsEnabled(LogLevel.Warning))
                        {
                            _logger.LogError("Didn't have covered this case at code");
                        }

                        break;

                }
            }
        }

        async Task SaveFile(byte[] bytes)
        {
            string filename = @"data.xlsx";
            await File.WriteAllBytesAsync(filename, bytes, CancellationToken.None);
        }
    }
}
