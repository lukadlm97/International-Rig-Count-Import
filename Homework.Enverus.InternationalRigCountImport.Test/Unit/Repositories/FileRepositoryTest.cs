using Homework.Enverus.InternationalRigCountImport.Core.Configurations;
using Homework.Enverus.InternationalRigCountImport.Core.Fetchers.Implementations;
using Homework.Enverus.InternationalRigCountImport.Test.Mocks;
using Homework.Enverus.Shared.Logging.Contracts;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Homework.Enverus.InternationalRigCountImport.Core.Repositories.Contracts;

namespace Homework.Enverus.InternationalRigCountImport.Test.Unit.Repositories
{

    public class FileRepositoryTest
    {
        private readonly Mock<IHighPerformanceLogger> _highPerformanceLogger;
        private readonly Mock<IFileRepository> _fileRepositoryMock;

        public FileRepositoryTest()
        {
            _highPerformanceLogger = MockHighPerformanceLogger.GetHighPerformanceLoggerMock();
            _fileRepositoryMock = MockFileRepository.GetVirtualFileRepositoryMock();
        }

        [Xunit.Theory]
        [InlineData("file name.csv", "a;b;c;d;")]
        [InlineData("file name.csv", "")]
        public async Task SaveFile_StringContent(string fileName, string content)
        {
            var repository = _fileRepositoryMock.Object;
            var saved = await repository.SaveFile(fileName, content);
            Assert.True(saved);
        }

        [Xunit.Theory]
        [InlineData("file name.xslx", new byte[2]{20,5})]
        [InlineData("file name.xslx", new byte[0] { })]
        public async Task SaveFile_BytesContent(string fileName, byte[] content)
        {
            var repository = _fileRepositoryMock.Object;
            var saved = await repository.SaveFile(fileName, content);
            Assert.True(saved);
        }
    }
}
