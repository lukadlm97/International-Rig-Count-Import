using Homework.Enverus.InternationalRigCountImport.Core.Configurations;
using Homework.Enverus.InternationalRigCountImport.Core.Exceptions;
using Homework.Enverus.InternationalRigCountImport.Core.Extensions;
using Homework.Enverus.InternationalRigCountImport.Core.Fetchers.Contracts;
using Homework.Enverus.InternationalRigCountImport.Core.Models;
using Homework.Enverus.InternationalRigCountImport.Core.Models.DTOs;
using Homework.Enverus.InternationalRigCountImport.Core.Models.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Homework.Enverus.InternationalRigCountImport.Core.Fetchers.Implementations
{
    public class BakerHughesFileProvider : IFileProvider
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ExternalDataSources _externalDataSource;
        private readonly ILogger<BakerHughesFileProvider> _logger;

        public BakerHughesFileProvider(IHttpClientFactory httpClientFactory,
            IOptions<ExternalDataSources> options, 
            ILogger<BakerHughesFileProvider> logger)
        {
            _httpClientFactory = httpClientFactory;
            _externalDataSource = options.Value;
            _logger = logger;
        }
        public async Task<OperationResult<RawFile>> 
            GetInternationalRigCount(string? dynamicUrl = default, CancellationToken cancellationToken = default)
        {
            if (dynamicUrl == default)
            {
                var staticFileUrl = _externalDataSource.StaticFileUrl;
                if (staticFileUrl == null || string.IsNullOrWhiteSpace(staticFileUrl))
                {
                    throw new MissingImporterOrExporterConfigurationsExceptions("Not provided endpoint for scraping for:" + Constants.BakerHughesRigCountImporterAndExporterName);
                }

                dynamicUrl = _externalDataSource.StaticFileUrl;
            }
           
            try
            {
                var client = _httpClientFactory.CreateClient(Constants.HttpClientName);
                var response = await client.GetAsync(dynamicUrl, cancellationToken);
                if (response.IsSuccessStatusCode)
                {
                    var file = await response.Content.ReadAsByteArrayAsync(cancellationToken);

                    return new OperationResult<RawFile>(OperationStatus.Ok,new RawFile(file));
                }
            }
            catch (Exception ex)
            {
                if (_logger.IsEnabled(LogLevel.Error))
                {
                    _logger.LogError("Error occurred at downloading file from URL", ex);
                }
                return new OperationResult<RawFile>(OperationStatus.Unknown, ex.Message);
            }

            return new OperationResult<RawFile>(OperationStatus.UnavailableAction);
        }
        
    }
}
