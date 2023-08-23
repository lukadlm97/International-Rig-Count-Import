
using Homework.Enverus.Shared.Logging.Contracts;
using Microsoft.Extensions.Logging;
using Moq;

namespace Homework.Enverus.InternationalRigCountImport.Test.Mocks
{
    public static class MockHighPerformanceLogger
    {
        public static Mock<IHighPerformanceLogger> GetHighPerformanceLoggerMock()
        {
            var mockLogger = new Mock<IHighPerformanceLogger>();

            mockLogger.Setup(r => r.Log(It.IsAny<string>(), It.IsAny<LogLevel>()));
            mockLogger.Setup(r => r.Log(It.IsAny<string>(), It.IsAny<Exception>(), It.IsAny<LogLevel>()));


            return mockLogger;
        }
    }
}
