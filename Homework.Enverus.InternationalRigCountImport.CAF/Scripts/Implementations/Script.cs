using Homework.Enverus.InternationalRigCountImport.CAF.Scripts.Contracts;
using Homework.Enverus.InternationalRigCountImport.Core.Models.Enums;
using Homework.Enverus.InternationalRigCountImport.Core.Services.Contracts;
using Homework.Enverus.Shared.Logging.Contracts;
using Microsoft.Extensions.Logging;

namespace Homework.Enverus.InternationalRigCountImport.CAF.Scripts.Implementations
{
    public class Script : ConsoleAppBase, IScript
    {
        private readonly IRigCountImporter _imported;
        private readonly IRigCountExporter _exporter;
        private readonly IHighPerformanceLogger _logger;

        public Script(IHighPerformanceLogger logger, IRigCountImporter importer, IRigCountExporter exporter)
        {
            _logger = logger;
            _imported = importer;
            _exporter = exporter;
        }
        
        [Command("importRigCount", "Command that perform fetching of rig count from predefined URL (from appsettings.json) and export that information to output.csv file by sent parameters.")]

        public async Task Import(bool? advancedHandling = null, bool? useArchive = null, int? year = null, int? rowPerYear = null,
            string? delimiter = null, CancellationToken cancellationToken = default)
        {
            _logger.Log("International Rig count service has started...", LogLevel.Information);

            var importResult = await _imported.Import(advancedHandling, useArchive, cancellationToken);
            switch (importResult.Status)
            {
                case OperationStatus.Ok:
                    _logger.Log("Import content successfully fetched from data source...", LogLevel.Information);

                    var exportResult = await _exporter.Export(year, rowPerYear, delimiter,
                        importResult.Result.DateDir, importResult.Result.TimeDir, cancellationToken);
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
