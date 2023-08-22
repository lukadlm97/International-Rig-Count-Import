using Homework.Enverus.InternationalRigCountImport.Core.Configurations;
using Homework.Enverus.InternationalRigCountImport.Core.Extensions;
using Homework.Enverus.InternationalRigCountImport.Core.Fetchers.Contracts;
using Homework.Enverus.InternationalRigCountImport.Core.Models.Enums;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Homework.Enverus.InternationalRigCountImport.Core.Repositories.Contracts;
using Microsoft.Extensions.Options;
using IFileProvider = Homework.Enverus.InternationalRigCountImport.Core.Fetchers.Contracts.IFileProvider;
using Homework.Enverus.InternationalRigCountImport.Core.Services.Contracts;

namespace Homework.Enverus.InternationalRigCountImport.Console.Services.Implementation
{
    public class InternationalRigCountBackgroundService : BackgroundService
    {
        private readonly ILogger<InternationalRigCountBackgroundService> _logger;
        private readonly IFilePathProvider _filePathProvider;
        private readonly IFileProvider _fileProvider;
        private readonly IExcelFileRepository _excelFileRepository;
        private readonly IRigCountExporter _rigCountExporter;
        private readonly AdvancedSettings _advancedSettings;

        public InternationalRigCountBackgroundService(IFilePathProvider filePathProvider,
            IFileProvider fileProvider,
            IExcelFileRepository excelFileRepository,
            IRigCountExporter rigCountExporter,
            IOptions<AdvancedSettings> options,
            ILogger<InternationalRigCountBackgroundService> logger)
        {
            _filePathProvider = filePathProvider;
            _fileProvider = fileProvider;
            _excelFileRepository = excelFileRepository;
            _rigCountExporter = rigCountExporter;
            _logger = logger;
            _advancedSettings= options.Value;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("International Rig Count Hosted Service running.");

            await DoWork(stoppingToken);
        }

        private async Task DoWork(CancellationToken stoppingToken)
        {
            _logger.LogInformation("International Rig Count Hosted Service is working.");

            var path = await _filePathProvider.GetFilePath(stoppingToken);
            var result = await _fileProvider.GetInternationalRigCount(path, stoppingToken);

            var advancedHandling = false;
            var dateDir = string.Empty;
            var timeDir = string.Empty;
            if (_advancedSettings.Enabled)
            {
                advancedHandling = true;
                if (_advancedSettings.ArchiveOldSamples)
                {
                    var now = DateTime.UtcNow;
                    dateDir = now.ToString(ExcelFileConstants.ExcelFileDirectoryDateFormat);
                    timeDir = now.ToString(ExcelFileConstants.ExcelFileDirectoryTimeFormat);
                }
            }
            switch (result.Status)
            {
                case OperationStatus.Ok:
                    if (_logger.IsEnabled(LogLevel.Information))
                    {
                        _logger.LogInformation("Successfully downloaded!!!");
                    }

                    
                    if (!await _excelFileRepository.SaveFile(result.Result.FileBytes, 
                            advancedHandling,
                            dateDir,
                            timeDir,
                            cancellationToken: stoppingToken))
                    {
                        if (_logger.IsEnabled(LogLevel.Error))
                        {
                            _logger.LogError("Problems occurred on excel file materialization!!!");
                            return;
                        }
                    }
                    if (_logger.IsEnabled(LogLevel.Information))
                    {
                        _logger.LogInformation("Excel file successfully saved at root location");
                    }

                    var stats = await _excelFileRepository.LoadFile(advancedHandling,
                        dateDir,
                        timeDir, 
                        stoppingToken);

                    if (!await _rigCountExporter.Write(stats, stoppingToken))
                    {
                        if (_logger.IsEnabled(LogLevel.Error))
                        {
                            _logger.LogError("Problems occurred on csv file creation!!!");
                            return;
                        }
                    }
                    if (_logger.IsEnabled(LogLevel.Information))
                    {
                        _logger.LogInformation("CSV file successfully saved at export location");
                    }
                    break;
                case OperationStatus.BadRequest:
                case OperationStatus.NotFound:
                case OperationStatus.UnavailableAction:
                case OperationStatus.Unknown:
                    if (_logger.IsEnabled(LogLevel.Error))
                    {
                        _logger.LogError("Operation completed with status error: "+result.Description);
                    }
                    break;
                default:
                    if (_logger.IsEnabled(LogLevel.Warning))
                    {
                        _logger.LogError("Didn't have covered this case at code");
                    }

                    break;
            }
            
        }
    }
}
