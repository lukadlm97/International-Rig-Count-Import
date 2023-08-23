using Homework.Enverus.InternationalRigCountImport.Core.Configurations;
using Homework.Enverus.InternationalRigCountImport.Core.Services.Contracts;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Homework.Enverus.InternationalRigCountImport.Core.Services.Implementations
{
    public class RigCountExporter : IRigCountExporter
    {
        private readonly ILogger<RigCountExporter> _logger;
        private readonly AdvancedSettings _advancedSettings;
        private readonly IExcelService _excelService;
        private readonly ICsvService _csvService;

        public RigCountExporter(ILogger<RigCountExporter> logger, 
            IExcelService excelService, 
            ICsvService csvService, 
            IOptions<AdvancedSettings> advancedOptions)
        {
            _logger = logger;
            _excelService = excelService;
            _csvService = csvService;
            _advancedSettings = advancedOptions.Value;
        }
        public async Task<bool> Export(int? year = null, 
            int? rowPerYear = null,
            string? delimiter = null, 
            string? dateDir = null, 
            string? timeDir = null, 
            CancellationToken cancellationToken = default)
        {
            if (_advancedSettings.Enabled)
            {
                if (dateDir == null || timeDir == null)
                {
                    throw new ArgumentNullException(dateDir == null ? nameof(dateDir) : nameof(timeDir));
                }
            }

            try
            {
                var stats = 
                    _excelService.LoadFile(_advancedSettings.Enabled, dateDir, timeDir);

                return await _csvService.SaveFile(stats, year, rowPerYear, delimiter, cancellationToken);
            }
            catch (Exception ex)
            {
                if (_logger.IsEnabled(LogLevel.Error))
                {
                    _logger.LogError("Problem on create export ",ex);
                }
            }
            return false;
        }
    }
}
