using Homework.Enverus.InternationalRigCountImport.Core.Configurations;
using Homework.Enverus.InternationalRigCountImport.Core.Models.Enums;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Homework.Enverus.InternationalRigCountImport.Core.Services.Contracts;

namespace Homework.Enverus.InternationalRigCountImport.Console.Services.Implementation
{
    public class InternationalRigCountBackgroundService : BackgroundService
    {
        private readonly ILogger<InternationalRigCountBackgroundService> _logger;
        private readonly IRigCountExporter _rigCountExporter;
        private readonly IRigCountImporter _rigCountImporter;

        public InternationalRigCountBackgroundService(ILogger<InternationalRigCountBackgroundService> logger,
            IRigCountImporter rigCountImporter,
            IRigCountExporter rigCountExporter, 
            IOptions<AdvancedSettings> options)
        {
            _logger = logger;
            _rigCountImporter = rigCountImporter;
            _rigCountExporter = rigCountExporter;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("International Rig Count Hosted Service running.");

            await DoWork(stoppingToken);
        }

        private async Task DoWork(CancellationToken stoppingToken)
        {
            _logger.LogInformation("International Rig Count Hosted Service is working.");

            var importResult = await _rigCountImporter.Import(null, null, stoppingToken);
            switch (importResult.Status)
            {
                case OperationStatus.Ok:
                    if (_logger.IsEnabled(LogLevel.Information))
                    {
                        _logger.LogInformation("Successfully imported!!!");
                    }

                    
                    if(!await _rigCountExporter.Export(null,null,null,
                           importResult.Result.DateDir, importResult.Result.TimeDir, stoppingToken))
                    {
                        if (_logger.IsEnabled(LogLevel.Error))
                        {
                            _logger.LogError("Problems occurred on export creation!!!");
                            return;
                        }
                    }
                    if (_logger.IsEnabled(LogLevel.Information))
                    {
                        _logger.LogInformation("CSV file successfully saved at export location");
                    }
                    break;
                case OperationStatus.BadRequest:
                case OperationStatus.NotFound:
                case OperationStatus.UnavailableAction:
                case OperationStatus.Unknown:
                    if (_logger.IsEnabled(LogLevel.Error))
                    {
                        _logger.LogError("Operation completed with status error: "+importResult.Description);
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
}
