using Homework.Enverus.InternationalRigCountImport.Core.Extensions;
using Homework.Enverus.InternationalRigCountImport.Core.Fetchers.Contracts;
using Homework.Enverus.InternationalRigCountImport.Core.Models.Enums;
using Homework.Enverus.InternationalRigCountImport.Core.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Globalization;
using Homework.Enverus.InternationalRigCountImport.Core.Services;

namespace Homework.Enverus.InternationalRigCountImport.Console.Services.Implementation
{
    public class InternationalRigCountBackgroundService : BackgroundService
    {
        public IServiceProvider _services { get; }
        private readonly ILogger<InternationalRigCountBackgroundService> _logger;

        public InternationalRigCountBackgroundService(IServiceProvider services,
            ILogger<InternationalRigCountBackgroundService> logger)
        {
            _services = services;
            _logger = logger;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("International Rig Count Hosted Service running.");

            await DoWork(stoppingToken);
        }

        private async Task DoWork(CancellationToken stoppingToken)
        {
            _logger.LogInformation("International Rig Count Hosted Service is working.");

            using (var scope = _services.CreateScope())
            {
                var scopedFileProvider =
                    scope.ServiceProvider
                        .GetRequiredService<IFileProvider>();
                var scopedExcelFileRepository =
                    scope.ServiceProvider
                        .GetRequiredService<IExcelFileRepository>();
                var scopedRigCountExporter =
                    scope.ServiceProvider
                        .GetRequiredService<IRigCountExporter>();

                var result = await scopedFileProvider.GetInternationalRigCount(stoppingToken);

                switch (result.Status)
                {
                    case OperationStatus.Ok:
                        if (_logger.IsEnabled(LogLevel.Information))
                        {
                            _logger.LogInformation("Successfully downloaded!!!");
                        }

                        var now = DateTime.UtcNow;
                        var dateDir = now.ToString(ExcelFileConstants.ExcelFileDirectoryDateFormat, CultureInfo.InvariantCulture);
                        var timeDir = now.ToString(ExcelFileConstants.ExcelFileDirectoryTimeFormat, CultureInfo.InvariantCulture);
                        if (!await scopedExcelFileRepository.SaveFile(result.Result.FileBytes,
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

                        var stats = await scopedExcelFileRepository.LoadFile(dateDir, timeDir, stoppingToken);

                        if (!await scopedRigCountExporter.Write(stats, stoppingToken))
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

        async Task SaveFile(byte[] bytes)
        {
            string filename = @"data.xlsx";
            await File.WriteAllBytesAsync(filename, bytes, CancellationToken.None);
        }
    }
}
