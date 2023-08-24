using Moq;
using Homework.Enverus.InternationalRigCountImport.Core.Repositories.Contracts;

namespace Homework.Enverus.InternationalRigCountImport.Test.Mocks
{
    public static class MockFileRepository
    {
        static Dictionary<string, byte[]> excelFileDirectory = new Dictionary<string, byte[]>();
        static Dictionary<string, string> csvFIleDirectory = new Dictionary<string, string>();

        public static Mock<IFileRepository> GetVirtualFileRepositoryMock()
        {
            var mockFileRepository = new Mock<IFileRepository>();

            mockFileRepository.Setup(r =>
                    r.SaveFile(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((string name, string content, CancellationToken cancellationToken) =>
                {
                    if (csvFIleDirectory.ContainsKey(name))
                    {
                        csvFIleDirectory[name] = content;
                        return true;
                    }

                    if (csvFIleDirectory.TryAdd(name, content))
                    {
                        return true;
                    }

                    return false;
                });


            mockFileRepository.Setup(r =>
                    r.SaveFile(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((string name, byte[] content, CancellationToken cancellationToken) =>
                {
                    if (excelFileDirectory.ContainsKey(name))
                    {
                        excelFileDirectory[name] = content;
                        return true;
                    }

                    if (excelFileDirectory.TryAdd(name, content))
                    {
                        return true;
                    }

                    return false;
                });

            return mockFileRepository;
        }
    }
}