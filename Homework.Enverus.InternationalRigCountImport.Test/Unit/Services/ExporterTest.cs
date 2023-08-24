using Homework.Enverus.InternationalRigCountImport.Core.Configurations;
using Homework.Enverus.InternationalRigCountImport.Core.Models.Enums;
using Homework.Enverus.InternationalRigCountImport.Core.Repositories.Contracts;
using Homework.Enverus.InternationalRigCountImport.Core.Services.Implementations;
using Homework.Enverus.InternationalRigCountImport.Test.Mocks;
using Homework.Enverus.InternationalRigCountImport.Test.Utilities;
using Homework.Enverus.Shared.Logging.Contracts;
using Microsoft.Extensions.Options;
using Moq;

namespace Homework.Enverus.InternationalRigCountImport.Test.Unit.Services
{
    public class ExporterTest
    {
        private readonly Mock<IHighPerformanceLogger> _highPerformanceLogger;
        private readonly Mock<IFileRepository> _fileRepositoryMock;

        public ExporterTest()
        {
            _highPerformanceLogger = MockHighPerformanceLogger.GetHighPerformanceLoggerMock();
            _fileRepositoryMock = MockFileRepository.GetVirtualFileRepositoryMock();
        }

        [Xunit.Fact]
        public async Task ExportCsv()
        {
            var excelService = RealProvider.GetExcelService(_highPerformanceLogger.Object, _fileRepositoryMock.Object);
            var csvService = RealProvider.GetCsvService(_highPerformanceLogger.Object, _fileRepositoryMock.Object);
            var advancedSettings = Options.Create(new AdvancedSettings() { });


            var exporeter =
                new RigCountExporter(_highPerformanceLogger.Object, excelService, csvService, advancedSettings);

            var result = await exporeter.Export(1,2,cancellationToken: CancellationToken.None);

            Assert.True(result.Status==OperationStatus.Ok);
        }

        /*
        Again constant secure us to obligatory have excel file (this must be tested manually)
        [Xunit.Fact]
        public async Task ExportCsv_CouldNotFoundDataSourceAtLocalFileSystem()
        {
            var excelService = RealProvider.GetExcelService(_highPerformanceLogger.Object, _fileRepositoryMock.Object);
            var csvService = RealProvider.GetCsvService(_highPerformanceLogger.Object, _fileRepositoryMock.Object);
            var advancedSettings = Options.Create(new AdvancedSettings() { });


            var exporeter =
                new RigCountExporter(_highPerformanceLogger.Object, excelService, csvService, advancedSettings);

            var result = await exporeter.Export(1, 2, cancellationToken: CancellationToken.None);

            Assert.True(result.Status == OperationStatus.Ok);
        }
        */
        [Xunit.Theory]
        [InlineData("", ",")]
        [InlineData("output.csv", "")]
        public async Task ExportCsv_MissingCSVName(string fileName, string delimiter)
        {
            var excelService = RealProvider.GetExcelService(_highPerformanceLogger.Object, _fileRepositoryMock.Object);
            var csvService = RealProvider.GetCsvService(_highPerformanceLogger.Object, _fileRepositoryMock.Object, new ExportDestinationSettings()
            {
                CsvSettings = new CsvSettings()
                {
                    FileName = fileName,
                    Delimiter = delimiter
                }
            });
            var advancedSettings = Options.Create(new AdvancedSettings() { });


            var exporeter =
                new RigCountExporter(_highPerformanceLogger.Object, excelService, csvService, advancedSettings);

            var result = await exporeter.Export(1, 2, cancellationToken: CancellationToken.None);

            Assert.True(result.Status != OperationStatus.Ok);
        }


    }
}
