using Homework.Enverus.InternationalRigCountImport.Core.Repositories.Contracts;
using Homework.Enverus.Shared.Logging.Contracts;
using Microsoft.Extensions.Logging;

namespace Homework.Enverus.InternationalRigCountImport.Core.Repositories.Implementations
{
    public class FileRepository:IFileRepository
    {
        private readonly IHighPerformanceLogger _logger;

        public FileRepository(IHighPerformanceLogger logger)
        {
            _logger = logger;
        }
        public async Task<bool> SaveFile(string path, byte[] bytes, CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(path))
                {
                    throw new ArgumentNullException(path);
                }
                await File.WriteAllBytesAsync(path, bytes, cancellationToken);

                return true;
            }
            catch (Exception ex)
            {
                _logger.Log("Error occurred at materialization of file...", ex, LogLevel.Error);

                return false;
            }
        }

        public async Task<bool> SaveFile(string path, string content, CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(path))
                {
                    throw new ArgumentNullException(path);
                }
                await File.WriteAllTextAsync(path, content, cancellationToken);

                return true;
            }
            catch (Exception ex)
            {
                _logger.Log("Error occurred at materialization of file...", ex, LogLevel.Error);

                return false;
            }
        }
    }
}
