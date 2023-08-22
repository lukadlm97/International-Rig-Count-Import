using Homework.Enverus.InternationalRigCountImport.Core.Configurations;
using Homework.Enverus.InternationalRigCountImport.Core.Repositories.Contracts;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Homework.Enverus.InternationalRigCountImport.Core.Repositories.Implementations
{
    public class CsvRepository : ICsvRepository
    {
        private readonly ILogger<CsvRepository> _logger;

        public CsvRepository(ILogger<CsvRepository> logger)
        {
            _logger = logger;
        }
        public async Task<bool> SaveFile(string content, string fileName, bool advancedHandling = false, string path = null, CancellationToken cancellationToken = default)
        {
            try
            {
                var fullPath = fileName;
                if (string.IsNullOrWhiteSpace(fileName))
                {
                    throw new ArgumentNullException(fileName);
                }

                if (advancedHandling)
                {
                    fullPath = Path.Combine(path, fileName);
                }
                await File.WriteAllTextAsync(fullPath, content, cancellationToken);
                return true;
            }
            catch (Exception ex)
            {
                if (_logger.IsEnabled(LogLevel.Error))
                {
                    _logger.LogError("Error occurred at materialization of excel file at file system", ex);
                }

                return false;
            }
        }
    }
}
