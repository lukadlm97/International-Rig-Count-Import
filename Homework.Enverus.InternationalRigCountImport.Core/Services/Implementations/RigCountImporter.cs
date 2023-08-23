using Homework.Enverus.InternationalRigCountImport.Core.Services.Contracts;
using Homework.Enverus.InternationalRigCountImport.Core.Fetchers.Contracts;
using Homework.Enverus.InternationalRigCountImport.Core.Models;
using Homework.Enverus.InternationalRigCountImport.Core.Models.DTOs;
using Microsoft.Extensions.Logging;
using Homework.Enverus.InternationalRigCountImport.Core.Configurations;
using Homework.Enverus.InternationalRigCountImport.Core.Extensions;
using Homework.Enverus.InternationalRigCountImport.Core.Models.Enums;
using Homework.Enverus.Shared.Logging.Contracts;
using Microsoft.Extensions.Options;

namespace Homework.Enverus.InternationalRigCountImport.Core.Services.Implementations
{
    public class RigCountImporter : IRigCountImporter
    {
        private readonly IHighPerformanceLogger _logger;
        private readonly IFilePathProvider _filePathProvider;
        private readonly IFileProvider _fileProvider;
        private readonly IExcelService _excelService;
        private readonly AdvancedSettings _advancedSettings;

        public RigCountImporter(IHighPerformanceLogger logger,
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
            try
            {
                var path = await _filePathProvider.GetFilePath(cancellationToken);
                if (string.IsNullOrWhiteSpace(path))
                {
                    _logger.Log("Unable to determinate URL for excel fetching by index page...", LogLevel.Warning);
                }

                var result = await _fileProvider.GetInternationalRigCount(path, cancellationToken);
                switch (result.Status)
                {
                    case OperationStatus.Ok:
                        _logger.Log("File are successfully downloaded from remote URL...", LogLevel.Information);

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
                            _logger.Log("Problems occurred on excel file materialization...", LogLevel.Error);
                            return new OperationResult<FileDirectory>(OperationStatus.UnavailableAction);
                        }

                        _logger.Log("Excel file successfully saved at configured location...", LogLevel.Information);
                        return new OperationResult<FileDirectory>(OperationStatus.Ok, new FileDirectory(dateDir, timeDir));
                    case OperationStatus.BadRequest:
                    case OperationStatus.NotFound:
                    case OperationStatus.UnavailableAction:
                    case OperationStatus.Unknown:

                        _logger.Log(string.Format("Operation completed with status error: {0}...", result.Description), LogLevel.Error);
                        return new OperationResult<FileDirectory>(OperationStatus.BadRequest);
                    default:

                        _logger.Log("Didn't have covered this case at code", LogLevel.Error);
                        return new OperationResult<FileDirectory>(OperationStatus.Unknown);
                }
            }
            catch (Exception ex)
            {

                _logger.Log("Error occurred on import data from data source...", ex, LogLevel.Error);
            }

            return new OperationResult<FileDirectory>(OperationStatus.BadRequest);
        }
    }
}
