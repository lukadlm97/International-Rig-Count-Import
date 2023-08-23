using Homework.Enverus.InternationalRigCountImport.Core.Models.Enums;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Homework.Enverus.InternationalRigCountImport.Core.Services.Contracts;
using Homework.Enverus.Shared.Logging.Contracts;

namespace Homework.Enverus.InternationalRigCountImport.Console.Services.Implementation
{
    public class InternationalRigCountBackgroundService : BackgroundService
    {
        private readonly IHighPerformanceLogger _logger;
        private readonly IRigCountExporter _rigCountExporter;
        private readonly IRigCountImporter _rigCountImporter;

        public InternationalRigCountBackgroundService(IHighPerformanceLogger logger,
            IRigCountImporter rigCountImporter,
            IRigCountExporter rigCountExporter)
        {
            _logger = logger;
            _rigCountImporter = rigCountImporter;
            _rigCountExporter = rigCountExporter;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await DoWork(stoppingToken);
        }

        private async Task DoWork(CancellationToken stoppingToken)
        {
            _logger.Log("International Rig count service has started...", LogLevel.Information);

            var importResult = await _rigCountImporter.Import(null, null, stoppingToken);
            switch (importResult.Status)
            {
                case OperationStatus.Ok:
                    _logger.Log("Import content successfully fetched from data source...", LogLevel.Information);

                    var exportResult = await _rigCountExporter.Export(null, null, null,
                        importResult.Result.DateDir, importResult.Result.TimeDir, stoppingToken);
                    switch (exportResult.Status)
                    {
                        case OperationStatus.Ok: 
                            _logger.Log("Export successfully created at file system...", LogLevel.Information);
                            break;
                        case OperationStatus.NotFound:
                            _logger.Log("Source data for export doesn\'t found at expected location...", LogLevel.Error);
                            break;
                        case OperationStatus.BadRequest:
                        case OperationStatus.UnavailableAction:
                        case OperationStatus.Unknown:
                            _logger.Log("Something goes wrong on export creation...", LogLevel.Error);
                            break;
                    }
                    break;
                case OperationStatus.BadRequest:
                case OperationStatus.NotFound:
                case OperationStatus.UnavailableAction:
                case OperationStatus.Unknown:
                    _logger.Log(string.Format("Import content is unavailable: {0}...", importResult.Description), LogLevel.Error);

                    break;
                default:
                    _logger.Log("This case isn't covered with appropriate logic...", LogLevel.Error);

                    break;
            }
            _logger.Log("International Rig count service has finished...", LogLevel.Information);
        }
    }
}
