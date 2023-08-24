using Homework.Enverus.InternationalRigCountImport.Core.Repositories.Contracts;
using Homework.Enverus.InternationalRigCountImport.Test.Mocks;
using Homework.Enverus.Shared.Logging.Contracts;
using Moq;
using Homework.Enverus.InternationalRigCountImport.Core.Services.Implementations;
using Homework.Enverus.InternationalRigCountImport.Core.Configurations;
using Homework.Enverus.InternationalRigCountImport.Core.Exceptions;
using Microsoft.Extensions.Options;

namespace Homework.Enverus.InternationalRigCountImport.Test.Unit.Services
{
    public class CsvServiceTest
    {
        private readonly Mock<IHighPerformanceLogger> _highPerformanceLogger;
        private readonly Mock<IFileRepository> _fileRepositoryMock;

        private  IReadOnlyList<IReadOnlyList<string>> list;
        public CsvServiceTest()
        {
            _highPerformanceLogger = MockHighPerformanceLogger.GetHighPerformanceLoggerMock();
            _fileRepositoryMock = MockFileRepository.GetVirtualFileRepositoryMock();
            list = new List<List<string>>()
            {
                new List<string>()
                {
                    "a","b","c","d"
                },
                new List<string>()
                {
                    "e","f","g","h"
                }
            };
        }

        [Xunit.Fact]
        public async Task SaveFile()
        {
            var rigRows = list;
            var repository = _fileRepositoryMock.Object;
            var logger = _highPerformanceLogger.Object; 
            var exporterSettings = Options.Create(new Exporter()
            {
                DataSourceSettings = new DataSourceSettings()
                {
                    ExcelWorkbookSettings = new ExcelWorkbookSettings()
                    {
                        RowsPerYear = 2,
                        Years = 1
                    }
                }, 
                ExportDestinationSettings = new ExportDestinationSettings()
              {
                  CsvSettings = new CsvSettings()
                  {
                      Delimiter = ",",
                      FileName = "output.csv"
                  }
              }
            });
            var csvRepo = new CsvService(logger, repository, exporterSettings);

            var saved = await csvRepo.SaveFile(rigRows, cancellationToken:CancellationToken.None);
            Assert.True(saved);
        }

        [Xunit.Theory]
        [InlineData( 3, 1, ",", "output.csv")]
        public async Task SaveFile_UnableToCreateExport_WrongDataSourceConfigs_RowNumberMismatch(
            int rowsPerYear,
            int years, 
            string delimiter, 
            string fileName)
        {
            var rigRows = list;
            var repository = _fileRepositoryMock.Object;
            var logger = _highPerformanceLogger.Object;
            var exporterSettings = Options.Create(new Exporter()
            {
                DataSourceSettings = new DataSourceSettings()
                {
                    ExcelWorkbookSettings = new ExcelWorkbookSettings()
                    {
                        RowsPerYear = rowsPerYear,
                        Years = years
                    }
                },
                ExportDestinationSettings = new ExportDestinationSettings()
                {
                    CsvSettings = new CsvSettings()
                    {
                        Delimiter = delimiter,
                        FileName = fileName
                    }
                }
            });
            var csvRepo = new CsvService(logger, repository, exporterSettings);
            Func<Task> act = () => csvRepo.SaveFile(rigRows, cancellationToken:CancellationToken.None);

            await Assert.ThrowsAsync<MissingRowsForFullExportException>(act);
        }
        [Xunit.Theory]
        [InlineData( null, 1, ",", "output.csv")]
        [InlineData( 3, null, ",", "output.csv")]
        [InlineData( -3, 1, ",", "output.csv")]
        [InlineData( 3, -1, ",", "output.csv")]
        public async Task SaveFile_UnableToCreateExport_WrongDataSourceConfigs_WrongResultSetRange(
            int? rowsPerYear,
            int? years,
            string delimiter,
            string fileName)
        {
            var rigRows = list;
            var repository = _fileRepositoryMock.Object;
            var logger = _highPerformanceLogger.Object;
            var exporterSettings = Options.Create(new Exporter()
            {
                DataSourceSettings = new DataSourceSettings()
                {
                    ExcelWorkbookSettings = new ExcelWorkbookSettings()
                    {
                    }
                },
                ExportDestinationSettings = new ExportDestinationSettings()
                {
                    CsvSettings = new CsvSettings()
                    {
                        Delimiter = delimiter,
                        FileName = fileName
                    }
                }
            });
            var csvRepo = new CsvService(logger, repository, exporterSettings);
            Func<Task> act = () => csvRepo.SaveFile(rigRows, rowsPerYear:rowsPerYear, years:years,  cancellationToken: CancellationToken.None);

            await Assert.ThrowsAsync<ArgumentException>(act);
        }

        [Xunit.Theory]
        [InlineData(2, 1, "", "output.csv")]
        public async Task SaveFile_UnableToCreateExport_WrongDataSourceConfigs_DelimiterIssue(
            int rowsPerYear,
            int years,
            string delimiter,
            string fileName)
        {
            var rigRows = list;
            var repository = _fileRepositoryMock.Object;
            var logger = _highPerformanceLogger.Object;
            var exporterSettings = Options.Create(new Exporter()
            {
                DataSourceSettings = new DataSourceSettings()
                {
                    ExcelWorkbookSettings = new ExcelWorkbookSettings()
                    {
                        RowsPerYear = rowsPerYear,
                        Years = years
                    }
                },
                ExportDestinationSettings = new ExportDestinationSettings()
                {
                    CsvSettings = new CsvSettings()
                    {
                        FileName = fileName
                    }
                }
            });
            var csvRepo = new CsvService(logger, repository, exporterSettings);
            Func<Task> act = () => csvRepo.SaveFile(rigRows, rowsPerYear: rowsPerYear, years: years, delimiter:delimiter, cancellationToken: CancellationToken.None);

            await Assert.ThrowsAsync<ArgumentNullException>(act);
        }
    }
}
