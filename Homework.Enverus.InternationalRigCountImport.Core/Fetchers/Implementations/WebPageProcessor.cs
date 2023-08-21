using Homework.Enverus.InternationalRigCountImport.Core.Configurations;
using Homework.Enverus.InternationalRigCountImport.Core.Extensions;
using Homework.Enverus.InternationalRigCountImport.Core.Fetchers.Contracts;
using Homework.Enverus.InternationalRigCountImport.Core.Models;
using Homework.Enverus.InternationalRigCountImport.Core.Models.DTOs;
using Homework.Enverus.InternationalRigCountImport.Core.Models.Enums;
using Microsoft.Extensions.Options;

namespace Homework.Enverus.InternationalRigCountImport.Core.Fetchers.Implementations
{
    public class WebPageProcessor : IWebPageProcessor
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ScrapingEndpoints _scrapingEndpoints;

        public WebPageProcessor(IHttpClientFactory httpClientFactory, IOptions<ScrapingEndpoints> options)
        {
            _httpClientFactory = httpClientFactory;
            _scrapingEndpoints = options.Value;
        }
        public async Task<OperationResult<RawFile>> 
            GetInternationalRigCount(CancellationToken cancellationToken)
        {
            var client = _httpClientFactory.CreateClient(Constants.HttpClientName);

            try
            {
                var response = await client.GetAsync(_scrapingEndpoints.BakerHughesrigCountUrl.FileUrl, cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    var file = await response.Content.ReadAsByteArrayAsync(cancellationToken);

                    return new OperationResult<RawFile>(OperationStatus.Ok,new RawFile(file));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new OperationResult<RawFile>(OperationStatus.Unknown, ex.Message);
            }

            return new OperationResult<RawFile>(OperationStatus.UnavailableAction);
        }
        
    }
}
