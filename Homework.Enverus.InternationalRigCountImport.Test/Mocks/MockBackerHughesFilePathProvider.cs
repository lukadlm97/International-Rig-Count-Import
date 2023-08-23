using Moq;
using Homework.Enverus.InternationalRigCountImport.Core.Fetchers.Contracts;

namespace Homework.Enverus.InternationalRigCountImport.Test.Mocks
{
  
    public static class MockBackerHughesFilePathProvider
    {
        public static Mock<IFilePathProvider> GetBackerHughesFilePathProviderMock()
        {
            var mockPathProvider = new Mock<IFilePathProvider>();

            mockPathProvider.Setup(r => 
                    r.GetFilePath(It.IsAny<CancellationToken>()))
                .ReturnsAsync("static-files/7240366e-61cc-4acb-89bf-86dc1a0dffe8");


            return mockPathProvider;
        }
    }
}
