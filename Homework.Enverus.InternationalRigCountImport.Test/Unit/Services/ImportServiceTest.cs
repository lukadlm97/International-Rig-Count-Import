using Homework.Enverus.InternationalRigCountImport.Core.Repositories.Contracts;
using Homework.Enverus.InternationalRigCountImport.Test.Mocks;
using Homework.Enverus.Shared.Logging.Contracts;
using Moq;
using Homework.Enverus.InternationalRigCountImport.Core.Configurations;
using Homework.Enverus.InternationalRigCountImport.Core.Fetchers.Contracts;
using Homework.Enverus.InternationalRigCountImport.Core.Models.Enums;
using Homework.Enverus.InternationalRigCountImport.Core.Services.Implementations;
using Homework.Enverus.InternationalRigCountImport.Test.Utilities;
using Microsoft.Extensions.Options;

namespace Homework.Enverus.InternationalRigCountImport.Test.Unit.Services
{
    public class ImportServiceTest
    {
        private readonly Mock<IHighPerformanceLogger> _highPerformanceLogger;
        private readonly Mock<IFileRepository> _fileRepositoryMock;
        private readonly Mock<IFilePathProvider> _filePathProviderMock;
        private readonly Mock<IFilePathProvider> _filePathProviderWithWorngOutputMock;
        
        public ImportServiceTest()
        {
            _highPerformanceLogger = MockHighPerformanceLogger.GetHighPerformanceLoggerMock();
            _fileRepositoryMock = MockFileRepository.GetVirtualFileRepositoryMock();
            _filePathProviderMock = MockBackerHughesFilePathProvider.GetBackerHughesFilePathProviderMock();
            _filePathProviderWithWorngOutputMock = MockBackerHughesFilePathProvider.GetBackerHughesFilePathProviderWithoutPathMock();
        }

        [Xunit.Fact]
        public async Task DownloadAndSaveExcelFile()
        {
            var excelService = RealProvider.GetExcelService(_highPerformanceLogger.Object, _fileRepositoryMock.Object);
            var fileProvider = RealProvider.GetPathProvider(_highPerformanceLogger.Object);
            var advancedSettings = Options.Create(new AdvancedSettings() { });
            var importer = new RigCountImporter(_highPerformanceLogger.Object, advancedSettings, _filePathProviderMock.Object, fileProvider, excelService);
            
            var result = await importer.Import(cancellationToken: CancellationToken.None);

            Assert.True(result.Status == OperationStatus.Ok);
        }

        [Xunit.Fact]
        public async Task DownloadAndSaveExcelFile_CouldNotFoundDataSourceAtUrl()
        {
            var excelService = RealProvider.GetExcelService(_highPerformanceLogger.Object, _fileRepositoryMock.Object);
            var externalDataSourcesSettings = new ExternalDataSources()
            {
                BaseUrl="https://bakerhughesrigcount.gcs-web.com",
                UserAgent = "PostmanRuntime/7.32.3"
            };
            var fileProvider = RealProvider.GetPathProvider(_highPerformanceLogger.Object, externalDataSourcesSettings);
            var advancedSettings = Options.Create(new AdvancedSettings() { });
            var importer = new RigCountImporter(_highPerformanceLogger.Object, advancedSettings, _filePathProviderWithWorngOutputMock.Object, fileProvider, excelService);

            var result = await importer.Import(cancellationToken: CancellationToken.None);

            Assert.True(result.Status != OperationStatus.Ok);
        }
        /*
        And this case could not be testes because we have constants which hold Excel file name
        This can be move to configurations also, but also its not needed
        [Xunit.Fact]
        public async Task DownloadAndSaveExcelFile_CouldNotSaveExcelFile()
        {
            var excelService = RealProvider.GetExcelService(_highPerformanceLogger.Object, _fileRepositoryMock.Object);
            var externalDataSourcesSettings = new ExternalDataSources()
            {
                BaseUrl = "https://bakerhughesrigcount.gcs-web.com",
                UserAgent = "PostmanRuntime/7.32.3"
            };
            var fileProvider = RealProvider.GetPathProvider(_highPerformanceLogger.Object, externalDataSourcesSettings);
            var advancedSettings = Options.Create(new AdvancedSettings() { });
            var importer = new RigCountImporter(_highPerformanceLogger.Object, advancedSettings, _filePathProviderMock.Object, fileProvider, excelService);

            var result = await importer.Import(cancellationToken: CancellationToken.None);

            Assert.True(result.Status != OperationStatus.Ok);
        }
        */

    }
}
