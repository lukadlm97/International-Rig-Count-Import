using Homework.Enverus.InternationalRigCountImport.Core.Configurations;
using Homework.Enverus.InternationalRigCountImport.Core.Fetchers.Implementations;
using Homework.Enverus.InternationalRigCountImport.Test.Mocks;
using Homework.Enverus.Shared.Logging.Contracts;
using Microsoft.Extensions.Options;
using Moq;

namespace Homework.Enverus.InternationalRigCountImport.Test.Unit.Fetchers
{
    public class BakerHughesFilePathProviderTest
    {
        private readonly Mock<IHighPerformanceLogger> _highPerformanceLogger;
        public BakerHughesFilePathProviderTest()
        {
            _highPerformanceLogger = MockHighPerformanceLogger.GetHighPerformanceLoggerMock();
        }

        [Xunit.Theory]
        [InlineData("intl-rig-count?c=79687&p=irol-rigcountsintl", "static-files/7240366e-61cc-4acb-89bf-86dc1a0dffe8")]
        [InlineData("qwertyui", "")]
        public async Task GetFilePath_WithDiffConfigs(string retrieveFileUrl, string expectedReturn)
        {
            var persistenceOptions = Options.Create(new ExternalDataSources()
            { 
                BaseUrl="https://bakerhughesrigcount.gcs-web.com",
                RetrieveFileUrl= retrieveFileUrl,
                StaticFileUrl="static-files/7240366e-61cc-4acb-89bf-86dc1a0dffe8",
                RetrieveTagFile= "Worldwide Rig Count Jul 2023.xlsx",
                UserAgent="PostmanRuntime/7.32.3"
            });
            var provider = new BakerHughesFilePathProvider(persistenceOptions, _highPerformanceLogger.Object);
            var result = await provider.GetFilePath(CancellationToken.None);
            
            Assert.Equal(expectedReturn, result);
        }
    }
}
