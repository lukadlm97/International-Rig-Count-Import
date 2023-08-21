using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Homework.Enverus.InternationalRigCountImport.Core.Repositories
{
    public class CsvRepository:ICsvRepository
    {
        private readonly ILogger<CsvRepository> _logger;

        public CsvRepository(ILogger<CsvRepository> logger)
        {
            _logger = logger;
        }
        public async Task<bool> SaveFile(string content, string fileName, CancellationToken cancellationToken = default)
        {
            try
            {
                await File.WriteAllTextAsync(fileName, content, cancellationToken);
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
