using System.Text;
using Homework.Enverus.InternationalRigCountImport.Core.Configurations;
using Homework.Enverus.InternationalRigCountImport.Core.Exceptions;
using Homework.Enverus.InternationalRigCountImport.Core.Repositories.Contracts;
using Homework.Enverus.InternationalRigCountImport.Core.Services.Contracts;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Homework.Enverus.InternationalRigCountImport.Core.Services.Implementations
{
    public class CsvService : ICsvService
    {
        private readonly ILogger<CsvService> _logger;
        private readonly IFileRepository _fileRepository;
        private readonly Exporter _exporterSettings;
        private readonly AdvancedSettings _advancedSettings;

        public CsvService(ILogger<CsvService> logger, 
            IFileRepository fileRepository,
            IOptions<Exporter> exporterOptions, 
            IOptions<AdvancedSettings> advancedOptions)
        {
            _logger = logger;
            _fileRepository = fileRepository;
            _exporterSettings = exporterOptions.Value;
            _advancedSettings = advancedOptions.Value;
        }
        
        public async Task<bool> SaveFile(IReadOnlyList<IReadOnlyList<string>> rigRows,
            int? years = null,
            int? rowsPerYear = null,
            string? delimiter = null,
            CancellationToken cancellationToken = default)
        {
            years ??= _exporterSettings?.DataSourceSettings?.ExcelWorkbookSettings?.Years;
            rowsPerYear ??= _exporterSettings?.DataSourceSettings?.ExcelWorkbookSettings?.RowsPerYear;
            
            if (years == null || rowsPerYear == null)
            {
                throw new ArgumentNullException(years == null? nameof(years) : nameof(rowsPerYear));
            }

            var rowCount = (int) (years * rowsPerYear);
            if (rigRows.Count() < rowCount)
            {
                throw new MissingRowsForFullExportException();
            }

            if (string.IsNullOrWhiteSpace(delimiter))
            {
                if (string.IsNullOrWhiteSpace(_exporterSettings?.ExportDestinationSettings?.CsvSettings?.Delimiter))
                {
                    throw new ArgumentNullException(nameof(delimiter));
                }
                delimiter = _exporterSettings?.ExportDestinationSettings?.CsvSettings?.Delimiter;
            }

            if (string.IsNullOrWhiteSpace(delimiter))
            {
                throw new ArgumentNullException(nameof(delimiter));
            }

            var selectedStats = rigRows.Take(rowCount);

            StringBuilder sb = new StringBuilder();
            foreach (var row in selectedStats)
            {
                sb.AppendLine(string.Join(delimiter, row));
            }

            var fullPath = _exporterSettings?.ExportDestinationSettings?.CsvSettings?.FileName;
            if (string.IsNullOrWhiteSpace(fullPath))
            {
                throw new ArgumentNullException(nameof(fullPath));
            }
            if (_advancedSettings.Enabled)
            {
                fullPath = Path.Combine(_advancedSettings.CsvExportLocation, fullPath);
            }


            return await _fileRepository.SaveFile(fullPath, sb.ToString(), cancellationToken);
        }
    }
}
