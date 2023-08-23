using Homework.Enverus.InternationalRigCountImport.Core.Repositories.Contracts;
using Microsoft.Extensions.Logging;

namespace Homework.Enverus.InternationalRigCountImport.Core.Repositories.Implementations
{
    public class FileRepository:IFileRepository
    {
        private readonly ILogger<FileRepository> _logger;

        public FileRepository(ILogger<FileRepository> logger)
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
                if (_logger.IsEnabled(LogLevel.Error))
                {
                    _logger.LogError("Error occurred at materialization of file", ex);
                }

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
                if (_logger.IsEnabled(LogLevel.Error))
                {
                    _logger.LogError("Error occurred at materialization of file", ex);
                }

                return false;
            }
        }
    }
}
