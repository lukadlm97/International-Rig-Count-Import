using Homework.Enverus.InternationalRigCountImport.Core.Configurations;
using Homework.Enverus.InternationalRigCountImport.Core.Models.DTOs;
using Homework.Enverus.InternationalRigCountImport.Core.Models;
using Homework.Enverus.InternationalRigCountImport.Core.Models.Enums;
using Homework.Enverus.InternationalRigCountImport.Core.Services.Contracts;
using Homework.Enverus.Shared.Logging.Contracts;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Homework.Enverus.InternationalRigCountImport.Core.Services.Implementations
{
    public class RigCountExporter : IRigCountExporter
    {
        private readonly IHighPerformanceLogger _logger;
        private readonly AdvancedSettings _advancedSettings;
        private readonly IExcelService _excelService;
        private readonly ICsvService _csvService;

        public RigCountExporter(IHighPerformanceLogger logger, 
            IExcelService excelService, 
            ICsvService csvService, 
            IOptions<AdvancedSettings> advancedOptions)
        {
            _logger = logger;
            _excelService = excelService;
            _csvService = csvService;
            _advancedSettings = advancedOptions.Value;
        }
        public async Task<OperationResult<ExportFileDirectory>> Export(int? year,
            int? rowPerYear,
            string? delimiter = null,
            string? dateDir = null,
            string? timeDir = null,
            CancellationToken cancellationToken = default)
        {
            string? csvDestination = _advancedSettings.CsvExportLocation;
            if (_advancedSettings.Enabled && _advancedSettings.ArchiveOldSamples)
            {
                if (dateDir == null || timeDir == null || string.IsNullOrWhiteSpace(csvDestination))
                {
                    throw new ArgumentNullException(dateDir == null ? nameof(dateDir) : 
                                                    timeDir== null ? nameof(timeDir) : 
                                                    csvDestination);
                }
            }

            try
            {
                var stats = 
                    _excelService.LoadFile(_advancedSettings.Enabled, dateDir, timeDir);

                if (stats == null || !stats.Any())
                {
                    _logger.Log("Stats for export doesn\'t found", LogLevel.Error);
                    return new OperationResult<ExportFileDirectory>(OperationStatus.NotFound);
                }

                if (await _csvService.SaveFile(stats, year, rowPerYear, delimiter,
                        _advancedSettings.Enabled, csvDestination, cancellationToken))
                {
                    return new OperationResult<ExportFileDirectory>(OperationStatus.Ok, csvDestination);
                }
            }
            catch (Exception ex)
            {
                _logger.Log("Problems occurred at export creation", ex, LogLevel.Error);
            }
            return new OperationResult<ExportFileDirectory>(OperationStatus.BadRequest);
        }
    }
}
