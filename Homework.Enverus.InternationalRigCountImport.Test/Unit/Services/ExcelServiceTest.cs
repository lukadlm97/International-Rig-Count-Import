using Homework.Enverus.InternationalRigCountImport.Core.Configurations;
using Homework.Enverus.InternationalRigCountImport.Core.Repositories.Contracts;
using Homework.Enverus.InternationalRigCountImport.Core.Services.Implementations;
using Homework.Enverus.InternationalRigCountImport.Test.Mocks;
using Homework.Enverus.Shared.Logging.Contracts;
using Microsoft.Extensions.Options;
using Moq;

namespace Homework.Enverus.InternationalRigCountImport.Test.Unit.Services
{
    public class ExcelServiceTest
    {
        private readonly Mock<IHighPerformanceLogger> _highPerformanceLogger;
        private readonly Mock<IFileRepository> _fileRepositoryMock;

        private IReadOnlyList<IReadOnlyList<string>> list;
        public ExcelServiceTest()
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

        [Xunit.Theory]
        [InlineData( new byte[2] { 20, 5 })]
        public async Task SaveFile(byte[] content)
        {
            var repository = _fileRepositoryMock.Object;
            var logger = _highPerformanceLogger.Object;
            var exporterSettings = Options.Create(new Exporter()
            {
            });
            var excelDirSettings = Options.Create(new ExcelDirectorySettings()
            {
               OriginalExcelRoot = ""
            });
            var excelService = new ExcelService(logger, exporterSettings, excelDirSettings, repository);

            var saved = await excelService.SaveFile(content, cancellationToken: CancellationToken.None);
            Assert.True(saved);
        }

        [Xunit.Fact]
        public async Task SaveFile_UnableToSaveFile_EmptyContent()
        {
            var content = Array.Empty<byte>();
            var repository = _fileRepositoryMock.Object;
            var logger = _highPerformanceLogger.Object;
            var exporterSettings = Options.Create(new Exporter()
            {
            });
            var excelDirSettings = Options.Create(new ExcelDirectorySettings()
            {
                OriginalExcelRoot = ""
            });
            var excelService = new ExcelService(logger, exporterSettings, excelDirSettings, repository);
            Func<Task> act = () => excelService.SaveFile(content, cancellationToken: CancellationToken.None);

            await Assert.ThrowsAsync<ArgumentNullException>(act);
        }

        [Xunit.Theory]
        [InlineData(new byte[2] { 20, 5 }, true, true, "2023-08-24", "17-11-45-111")]
        [InlineData(new byte[2] { 20, 5 }, true, false, "", "")]
        public async Task SaveFile_AdvancedHandling(byte[] fileContent, bool advancedHandling, bool useArchive, string dateDir, string timeDir)
        {
            var repository = _fileRepositoryMock.Object;
            var logger = _highPerformanceLogger.Object;
            var exporterSettings = Options.Create(new Exporter()
            {
               
            });
            var excelDirSettings = Options.Create(new ExcelDirectorySettings()
            {
                OriginalExcelRoot = "something"
            });
            var excelService = new ExcelService(logger, exporterSettings, excelDirSettings, repository);

            var saved = await excelService.SaveFile(fileContent, advancedHandling, 
                useArchive, dateDir, timeDir, cancellationToken: CancellationToken.None);
            Assert.True(saved);
        }

        [Xunit.Theory]
        [InlineData(new byte[2] { 20, 5 }, true, true, "2023-08-24", "17-11-45-111", true)]
        [InlineData(new byte[2] { 20, 5 }, true, false, "", "", false)]
        public async Task SaveFile_AdvancedHandling_MissingOriginalExcelRoot(byte[] fileContent, bool advancedHandling, bool useArchive, string dateDir, string timeDir, bool expected)
        {
            var repository = _fileRepositoryMock.Object;
            var logger = _highPerformanceLogger.Object;
            var exporterSettings = Options.Create(new Exporter()
            {

            });
            var excelDirSettings = Options.Create(new ExcelDirectorySettings()
            {
                OriginalExcelRoot = string.Empty
            });
            var excelService = new ExcelService(logger, exporterSettings, excelDirSettings, repository);

            var saved = await excelService.SaveFile(fileContent, advancedHandling,
                useArchive, dateDir, timeDir, cancellationToken: CancellationToken.None);
            Assert.Equal(expected, saved);
        }

        [Xunit.Theory]
        [InlineData(new byte[2] { 20, 5 }, true, true, "", "17-11-45-111")]
        [InlineData(new byte[2] { 20, 5 }, true, true, "2023-08-24", "")]
        public async Task SaveFile_AdvancedHandling_WrongConfigs(byte[] fileContent, bool advancedHandling, bool useArchive, string dateDir, string timeDir)
        {
            var repository = _fileRepositoryMock.Object;
            var logger = _highPerformanceLogger.Object;
            var exporterSettings = Options.Create(new Exporter()
            {

            });
            var excelDirSettings = Options.Create(new ExcelDirectorySettings()
            {
                OriginalExcelRoot = "something"
            });
            var excelService = new ExcelService(logger, exporterSettings, excelDirSettings, repository);

            var saved = await excelService.SaveFile(fileContent, advancedHandling,
                useArchive, dateDir, timeDir, cancellationToken: CancellationToken.None);
            Assert.False(saved);
        }


        [Xunit.Fact]
        public void LoadExcelFile()
        {
            var repository = _fileRepositoryMock.Object;
            var logger = _highPerformanceLogger.Object;
            var exporterSettings = Options.Create(new Exporter()
            {
                DataSourceSettings = new DataSourceSettings()
                { 
                    ExcelWorkbookSettings = new ExcelWorkbookSettings()
                    {
                      
                        Worksheet = "Worldwide_Rigcount",
                        StartRow = 7,
                        StartColumn = 2,
                        EndRow = 740,
                        EndColumn = 11
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
            var excelDirSettings = Options.Create(new ExcelDirectorySettings()
            {
                OriginalExcelRoot = ""
            });
            var excelService = new ExcelService(logger, exporterSettings, excelDirSettings, repository);

            var loadedItems = excelService.LoadFile();
            Assert.True(loadedItems.Any());
        }

        [Xunit.Theory]
        [InlineData("", 7, 2, 740, 11)]
        [InlineData("Worldwide_Rigcount", -7, 2, 740, 11)]
        [InlineData("Worldwide_Rigcount", 7, -2, 740, 11)]
        [InlineData("Worldwide_Rigcount", 7, 2, -740, 11)]
        [InlineData("Worldwide_Rigcount", 7, 2, 740, -11)]
        public void LoadExcelFile_WrongConfigs(string worksheet, int startRow, int startColumn, int endRow, int endColumn)
        {
            var repository = _fileRepositoryMock.Object;
            var logger = _highPerformanceLogger.Object;
            var exporterSettings = Options.Create(new Exporter()
            {
                DataSourceSettings = new DataSourceSettings()
                {
                    ExcelWorkbookSettings = new ExcelWorkbookSettings()
                    {
                        Worksheet = worksheet,
                        StartRow = startRow,
                        StartColumn = startColumn,
                        EndRow = endRow,
                        EndColumn = endColumn
                    }
                }
            });
            var excelDirSettings = Options.Create(new ExcelDirectorySettings()
            {
                OriginalExcelRoot = ""
            });
            var excelService = new ExcelService(logger, exporterSettings, excelDirSettings, repository);

            var loadedItems = excelService.LoadFile();
            Assert.False(loadedItems.Any());
        }

        /*
        This case is little tricky to test because we need file system with subdirectories
        [Xunit.Theory]
        [InlineData("Worldwide_Rigcount", 7, 2, 740, 11)]
        public async Task LoadExcelFile_AdvancedHandling(string worksheet, int startRow, int startColumn, int endRow, int endColumn)
        {
            var repository = _fileRepositoryMock.Object;
            var logger = _highPerformanceLogger.Object;
            var exporterSettings = Options.Create(new Exporter()
            {
                DataSourceSettings = new DataSourceSettings()
                {
                    ExcelWorkbookSettings = new ExcelWorkbookSettings()
                    {
                        Worksheet = worksheet,
                        StartRow = startRow,
                        StartColumn = startColumn,
                        EndRow = endRow,
                        EndColumn = endColumn
                    }
                }
            });
            var excelDirSettings = Options.Create(new ExcelDirectorySettings()
            {
                OriginalExcelRoot = "BakerHughesRigCount"
            });
            var excelService = new ExcelService(logger, exporterSettings, excelDirSettings, repository);

            var loadedItems = excelService.LoadFile(true, "2023-08-21", "17-39-33-808");
            Assert.False(loadedItems.Any());
        }
        */
    }
}
