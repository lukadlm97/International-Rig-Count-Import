using Homework.Enverus.InternationalRigCountImport.Core.Configurations;
using Homework.Enverus.InternationalRigCountImport.Core.Fetchers.Implementations;
using Homework.Enverus.InternationalRigCountImport.Test.Mocks;
using Homework.Enverus.Shared.Logging.Contracts;
using Microsoft.Extensions.Options;
using Moq;
using Homework.Enverus.InternationalRigCountImport.Core.Models.Enums;

namespace Homework.Enverus.InternationalRigCountImport.Test.Unit.Fetchers
{
    public class BackerHughesFileProviderTest
    {
        private readonly Mock<IHighPerformanceLogger> _highPerformanceLogger;
        private readonly Mock<IHttpClientFactory> _httpClientFactory;
        public BackerHughesFileProviderTest()
        {
            _highPerformanceLogger = MockHighPerformanceLogger.GetHighPerformanceLoggerMock();
            _httpClientFactory = MockHttpClientFactory.GetHttpClientFactoryMock();
        }

        [Xunit.Theory]
        [InlineData("static-files/7240366e-61cc-4acb-89bf-86dc1a0dffe8", OperationStatus.Ok)]
        [InlineData("qwertyui", OperationStatus.UnavailableAction)]
        public async Task GetFilePath_WithDiffConfigs(string staticFileUrl, OperationStatus expectedReturn)
        {
            var persistenceOptions = Options.Create(new ExternalDataSources()
            {
                StaticFileUrl = staticFileUrl,
                RetrieveTagFile = "Worldwide Rig Count Jul 2023.xlsx",
                UserAgent = "PostmanRuntime/7.32.3"
            });
            var fileProvider = new BakerHughesFileProvider(_httpClientFactory.Object, persistenceOptions,
                _highPerformanceLogger.Object);

            var result = await fileProvider.GetInternationalRigCount(string.Empty, CancellationToken.None);

            Assert.Equal(result.Status, expectedReturn);
        }

        [Xunit.Theory]
        [InlineData("static-files/7240366e-61cc-4acb-89bf-86dc1a0dffe8", true)]
        [InlineData("qwertyui", false)]
        public async Task GetFilePath_ExpectedFile(string staticFileUrl, bool isOk)
        {
            var factory  = MockHttpClientFactory.GetHttpClientFactoryWithDataMock();
            var persistenceOptions = Options.Create(new ExternalDataSources()
            {
                StaticFileUrl = staticFileUrl,
                RetrieveTagFile = "Worldwide Rig Count Jul 2023.xlsx",
                UserAgent = "PostmanRuntime/7.32.3"
            });
            var fileProvider = new BakerHughesFileProvider(factory.Object, persistenceOptions,
                _highPerformanceLogger.Object);

            var result = await fileProvider.GetInternationalRigCount(string.Empty, CancellationToken.None);

            if (!isOk)
            {
                Assert.Equal(result.Result, null);
                return;
            }
            Assert.True(result.Result.FileBytes != null && result.Result.FileBytes.Any());
        }
    }
}
