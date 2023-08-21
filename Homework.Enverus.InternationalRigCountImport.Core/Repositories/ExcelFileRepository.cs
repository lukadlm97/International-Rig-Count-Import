using ClosedXML.Excel;
using Homework.Enverus.InternationalRigCountImport.Core.Configurations;
using Homework.Enverus.InternationalRigCountImport.Core.Extensions;
using Homework.Enverus.InternationalRigCountImport.Core.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Homework.Enverus.InternationalRigCountImport.Core.Repositories
{
    public class ExcelFileRepository:IExcelFileRepository
    {
        private readonly ExcelDirectorySettings _excelDirectorySettings;
        private readonly ILogger<ExcelFileRepository> _logger;
        private readonly ExcelWorkbookSettings _excelWorkbookSettings;

        public ExcelFileRepository(ILogger<ExcelFileRepository> logger, 
            IOptions<ExcelDirectorySettings> options, 
            IOptions<ExcelWorkbookSettings> wbOptions)
        {
            _logger = logger;
            _excelDirectorySettings = options.Value;
            _excelWorkbookSettings = wbOptions.Value;
        }
        public async Task<bool> SaveFile(byte[] file, string dateDirectory, string timeDirectory, CancellationToken cancellationToken = default)
        {
            try
            {
                var directoryPath = Path.Combine(_excelDirectorySettings.Root, dateDirectory, timeDirectory); 
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                var fullPath = Path.Combine(directoryPath, ExcelFileConstants.ExcelFileName);
                await File.WriteAllBytesAsync(fullPath, file, cancellationToken);

                return true;
            }
            catch (Exception ex)
            {
                if (_logger.IsEnabled(LogLevel.Error))
                {
                    _logger.LogError("Error occurred at materialization of excel file at file system",ex);
                }

                return false;
            }
        }

        public async Task<IReadOnlyList<IReadOnlyList<string>>> LoadFile(string dateDirectory, string timeDirectory, CancellationToken cancellationToken = default)
        {
            var stats = new List<List<string>>();
            try
            {
                var directoryPath = Path.Combine(_excelDirectorySettings.Root, dateDirectory, timeDirectory);
                var fullPath = Path.Combine(directoryPath, ExcelFileConstants.ExcelFileName);

                var wb = new XLWorkbook(fullPath);
                var ws = wb.Worksheet(_excelWorkbookSettings.Worksheet);

                for (int i = _excelWorkbookSettings.StartRow; i < _excelWorkbookSettings.EndRow; i++)
                {
                    var row = new List<string>();
                    for (int j = _excelWorkbookSettings.StartColumn; j < _excelWorkbookSettings.EndColumn; j++)
                    {
                        row.Add(ws.Cell(i,j).Value.ToString());
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
