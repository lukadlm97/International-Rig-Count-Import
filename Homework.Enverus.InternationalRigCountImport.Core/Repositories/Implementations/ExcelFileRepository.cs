using ClosedXML.Excel;
using Homework.Enverus.InternationalRigCountImport.Core.Configurations;
using Homework.Enverus.InternationalRigCountImport.Core.Extensions;
using Homework.Enverus.InternationalRigCountImport.Core.Repositories.Contracts;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Homework.Enverus.InternationalRigCountImport.Core.Repositories.Implementations
{
    public class ExcelFileRepository : IExcelFileRepository
    {
        private readonly ILogger<ExcelFileRepository> _logger;
        private readonly Exporter _exporter;
        private readonly ExcelDirectorySettings _excelDirectorySettings;

        public ExcelFileRepository(ILogger<ExcelFileRepository> logger,
            IOptions<Exporter> exporterOptions,
            IOptions<ExcelDirectorySettings> excelDirectoryOptions)
        {
            _logger = logger;
            _exporter = exporterOptions.Value;
            _excelDirectorySettings = excelDirectoryOptions.Value;
        }
        public async Task<bool> SaveFile(byte[] file,
            bool advancedHandling = false,
            string dateDirectory = default,
            string timeDirectory = default,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var fullPath = ExcelFileConstants.ExcelFileName;
                if (advancedHandling)
                {
                    string directoryPath = string.Empty;
                    if (string.IsNullOrWhiteSpace(dateDirectory) || string.IsNullOrWhiteSpace(timeDirectory))
                    {

                         directoryPath = Path.Combine(_excelDirectorySettings.OriginalExcelRoot);
                    }
                    else
                    {
                         directoryPath = Path.Combine(_excelDirectorySettings.OriginalExcelRoot, dateDirectory, timeDirectory);
                    }
              
                    if (!Directory.Exists(directoryPath))
                    {
                       Directory.CreateDirectory(directoryPath);
                    }

                    fullPath = Path.Combine(directoryPath, ExcelFileConstants.ExcelFileName);
                }

                await File.WriteAllBytesAsync(fullPath, file, cancellationToken);

                return true;
            }
            catch (Exception ex)
            {
                if (_logger.IsEnabled(LogLevel.Error))
                {
                    _logger.LogError("Error occurred at materialization of excel file at file system", ex);
                }

                return false;
            }
        }

        public async Task<IReadOnlyList<IReadOnlyList<string>>>
            LoadFile(bool advancedHandling = false,
                string dateDirectory = default,
                string timeDirectory = default,
                CancellationToken cancellationToken = default)
        {
            var stats = new List<List<string>>();
            try
            {
                var fullPath = ExcelFileConstants.ExcelFileName;
                if (advancedHandling)
                {
                    string directoryPath = string.Empty;
                    if (string.IsNullOrWhiteSpace(dateDirectory) || string.IsNullOrWhiteSpace(timeDirectory))
                    {

                        directoryPath = Path.Combine(_excelDirectorySettings.OriginalExcelRoot);
                    }
                    else
                    {
                        directoryPath = Path.Combine(_excelDirectorySettings.OriginalExcelRoot, dateDirectory, timeDirectory);
                    }

                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    fullPath = Path.Combine(directoryPath, ExcelFileConstants.ExcelFileName);
                }

                var wb = new XLWorkbook(fullPath);
                var ws = wb.Worksheet(_exporter.DataSourceSettings.ExcelWorkbookSettings.Worksheet);

                for (int i = _exporter.DataSourceSettings.ExcelWorkbookSettings.StartRow; i < _exporter.DataSourceSettings.ExcelWorkbookSettings.EndRow; i++)
                {
                    var row = new List<string>();
                    for (int j = _exporter.DataSourceSettings.ExcelWorkbookSettings.StartColumn; j < _exporter.DataSourceSettings.ExcelWorkbookSettings.EndColumn; j++)
                    {
                        row.Add(ws.Cell(i, j).Value.ToString());
                    }
                    stats.Add(row);
                }

            }
            catch (Exception ex)
            {
                if (_logger.IsEnabled(LogLevel.Error))
                {
                    _logger.LogError("Error occurred at materialization of excel file at file system", ex);
                }
            }

            return stats;
        }
    }
}
