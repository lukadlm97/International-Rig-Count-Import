using ClosedXML.Excel;
using Homework.Enverus.InternationalRigCountImport.Core.Configurations;
using Homework.Enverus.InternationalRigCountImport.Core.Extensions;
using Homework.Enverus.InternationalRigCountImport.Core.Repositories.Contracts;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Homework.Enverus.InternationalRigCountImport.Core.Services.Contracts;
using Homework.Enverus.Shared.Logging.Contracts;

namespace Homework.Enverus.InternationalRigCountImport.Core.Services.Implementations
{
    public class ExcelService : IExcelService
    {
        private readonly IHighPerformanceLogger _logger;
        private readonly Exporter _exporter;
        private readonly ExcelDirectorySettings _excelDirectorySettings;
        private readonly IFileRepository _fileRepository;

        public ExcelService(IHighPerformanceLogger logger,
            IOptions<Exporter> exporterOptions,
            IOptions<ExcelDirectorySettings> excelDirectoryOptions,
            IFileRepository fileRepository)
        {
            _logger = logger;
            _exporter = exporterOptions.Value;
            _excelDirectorySettings = excelDirectoryOptions.Value;
            _fileRepository = fileRepository;
        }
        public async Task<bool> SaveFile(byte[] file,
            bool? advancedHandling = null,
            bool? useArchive = null,
            string? dateDirectory = null,
            string? timeDirectory = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var fullPath = ExcelFileConstants.ExcelFileName;
                if (advancedHandling ?? false)
                {
                    string directoryPath = string.Empty;
                    if (useArchive ?? false)
                    {
                        if (string.IsNullOrWhiteSpace(dateDirectory) || string.IsNullOrWhiteSpace(timeDirectory))
                        {
                            throw new ArgumentNullException(dateDirectory==null ? nameof(dateDirectory) : nameof(timeDirectory));
                        }
                        directoryPath = Path.Combine(_excelDirectorySettings.OriginalExcelRoot, dateDirectory, timeDirectory);
                    }
                    else
                    {
                        directoryPath = Path.Combine(_excelDirectorySettings.OriginalExcelRoot);
                    }

                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }
                    fullPath = Path.Combine(directoryPath, ExcelFileConstants.ExcelFileName);
                }

                return await _fileRepository.SaveFile(fullPath, file, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.Log("Error occurred at materialization of excel file at file system", ex, LogLevel.Error);

                return false;
            }
        }

        public IReadOnlyList<IReadOnlyList<string>> LoadFile(bool advancedHandling = false,
                                                                string? dateDirectory = null,
                                                                string? timeDirectory = default)
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

                string? worksheet = _exporter?.DataSourceSettings?.ExcelWorkbookSettings?.Worksheet;
                int? startRow = _exporter?.DataSourceSettings?.ExcelWorkbookSettings?.StartRow;
                int? endRow = _exporter?.DataSourceSettings?.ExcelWorkbookSettings?.EndRow;
                int? startColumn = _exporter?.DataSourceSettings?.ExcelWorkbookSettings?.StartColumn;
                int? endColumn = _exporter?.DataSourceSettings?.ExcelWorkbookSettings?.EndColumn;

                if (string.IsNullOrWhiteSpace(worksheet) ||
                    startRow == null || endRow == null ||
                    startColumn == null || endColumn == null)
                {
                    throw new ArgumentNullException();
                }

                var wb = new XLWorkbook(fullPath);
                var ws = wb.Worksheet(worksheet);

                for (int i = (int) startRow; i < (int) endRow; i++)
                {
                    var row = new List<string>();

                    for (int j = (int) startColumn; j < (int) endColumn; j++)
                    {
                        row.Add(ws.Cell(i, j).Value.ToString());
                    }
                    stats.Add(row);
                }

            }
            catch (Exception ex)
            {
                _logger.Log("Error occurred at loading/manipulating excel file from file system", ex, LogLevel.Error);
            }

            return stats;
        }

        
    }
}
