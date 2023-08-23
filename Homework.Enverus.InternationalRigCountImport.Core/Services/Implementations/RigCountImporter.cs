using Homework.Enverus.InternationalRigCountImport.Core.Services.Contracts;
using Homework.Enverus.InternationalRigCountImport.Core.Fetchers.Contracts;
using Homework.Enverus.InternationalRigCountImport.Core.Models;
using Homework.Enverus.InternationalRigCountImport.Core.Models.DTOs;
using Microsoft.Extensions.Logging;
using Homework.Enverus.InternationalRigCountImport.Core.Configurations;
using Homework.Enverus.InternationalRigCountImport.Core.Extensions;
using Homework.Enverus.InternationalRigCountImport.Core.Models.Enums;
using Microsoft.Extensions.Options;

namespace Homework.Enverus.InternationalRigCountImport.Core.Services.Implementations
{
    public class RigCountImporter : IRigCountImporter
    {
        private readonly ILogger<RigCountImporter> _logger;
        private readonly IFilePathProvider _filePathProvider;
        private readonly IFileProvider _fileProvider;
        private readonly IExcelService _excelService;
        private readonly AdvancedSettings _advancedSettings;

        public RigCountImporter(ILogger<RigCountImporter> logger,
            IOptions<AdvancedSettings> advancedOptions,
            IFilePathProvider filePathProvider,
            IFileProvider fileProvider,
            IExcelService excelService)
        {
            _logger = logger;
            _filePathProvider = filePathProvider;
            _fileProvider = fileProvider;
            _excelService = excelService;
            _advancedSettings = advancedOptions.Value;
        }

        public async Task<OperationResult<FileDirectory>> Import(bool? advancedHandling = false,
            bool? useArchive = false,
            CancellationToken cancellationToken = default)
        {
            var path = await _filePathProvider.GetFilePath(cancellationToken);
            if (string.IsNullOrWhiteSpace(path))
            {
                if (_logger.IsEnabled(LogLevel.Warning))
                {
                    _logger.LogWarning("unable to determinate URL for excel fetching by index page");
                }
            }

            var result = await _fileProvider.GetInternationalRigCount(path, cancellationToken);
            switch (result.Status)
            {
                case OperationStatus.Ok:
                    if (_logger.IsEnabled(LogLevel.Information))
                    {
                        _logger.LogInformation("Successfully downloaded!!!");
                    }

                    string? dateDir = null;
                    string? timeDir = null;
                    advancedHandling ??= _advancedSettings.Enabled;
                    useArchive ??= _advancedSettings.ArchiveOldSamples;
                    if ((bool)advancedHandling && (bool)useArchive)
                    {
                        DateTime now = DateTime.UtcNow;
                        dateDir = now.ToString(ExcelFileConstants.ExcelFileDirectoryDateFormat);
                        timeDir = now.ToString(ExcelFileConstants.ExcelFileDirectoryTimeFormat);
                    }

                    if (!await _excelService.SaveFile(result.Result.FileBytes,
                            advancedHandling, useArchive, dateDir,
                            timeDir, cancellationToken))
                    {
                        if (_logger.IsEnabled(LogLevel.Error))
                        {
                            _logger.LogError("Problems occurred on excel file materialization!!!");
                            return new OperationResult<FileDirectory>(OperationStatus.UnavailableAction);
                        }
                    }

                    if (_logger.IsEnabled(LogLevel.Information))
                    {
                        _logger.LogInformation("Excel file successfully saved at root location");
                    }

                    return new OperationResult<FileDirectory>(OperationStatus.Ok, new FileDirectory(dateDir, timeDir));
                    break;
                case OperationStatus.BadRequest:
                case OperationStatus.NotFound:
                case OperationStatus.UnavailableAction:
                case OperationStatus.Unknown:
                    if (_logger.IsEnabled(LogLevel.Error))
                    {
                        _logger.LogError("Operation completed with status error: " + result.Description);
                    }

                    return new OperationResult<FileDirectory>(OperationStatus.BadRequest);
                    break;
                default:
                    if (_logger.IsEnabled(LogLevel.Warning))
                    {
                        _logger.LogError("Didn't have covered this case at code");
                    }

                    return new OperationResult<FileDirectory>(OperationStatus.Unknown);
                    break;
            }
        }

        public Task<OperationResult<FileDirectory>> Import(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
