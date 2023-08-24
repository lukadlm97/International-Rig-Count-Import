using System.Text;
using Homework.Enverus.InternationalRigCountImport.Core.Configurations;
using Homework.Enverus.InternationalRigCountImport.Core.Exceptions;
using Homework.Enverus.InternationalRigCountImport.Core.Repositories.Contracts;
using Homework.Enverus.InternationalRigCountImport.Core.Services.Contracts;
using Homework.Enverus.Shared.Logging.Contracts;
using Microsoft.Extensions.Options;

namespace Homework.Enverus.InternationalRigCountImport.Core.Services.Implementations
{
    public class CsvService : ICsvService
    {
        private readonly IFileRepository _fileRepository;
        private readonly Exporter _exporterSettings;

        public CsvService(IHighPerformanceLogger logger, 
            IFileRepository fileRepository,
            IOptions<Exporter> exporterOptions)
        {
            _fileRepository = fileRepository;
            _exporterSettings = exporterOptions.Value;
        }
        
        public async Task<bool> SaveFile(IReadOnlyList<IReadOnlyList<string>> rigRows,
            int? years = null,
            int? rowsPerYear = null,
            string? delimiter = null,
            bool? advancedHandling = null,
            string? csvLocation = null,
            CancellationToken cancellationToken = default)
        {
            years ??= _exporterSettings?.DataSourceSettings?.ExcelWorkbookSettings?.Years;
            rowsPerYear ??= _exporterSettings?.DataSourceSettings?.ExcelWorkbookSettings?.RowsPerYear;
            
            if (years == null || rowsPerYear == null || years <= 0 || rowsPerYear <= 0)
            {
                throw new ArgumentException(years == null? nameof(years) : nameof(rowsPerYear));
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
          
            if (string.IsNullOrWhiteSpace(_exporterSettings?.ExportDestinationSettings?.CsvSettings?.Delimiter))
            {
                throw new ArgumentNullException(nameof(delimiter));
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
            if (advancedHandling ?? false)
            {
                if (string.IsNullOrWhiteSpace(csvLocation))
                {
                    throw new ArgumentNullException(nameof(csvLocation));
                }
                fullPath = Path.Combine(csvLocation, fullPath);
            }
            
            return await _fileRepository.SaveFile(fullPath, sb.ToString(), cancellationToken);
        }
    }
}
