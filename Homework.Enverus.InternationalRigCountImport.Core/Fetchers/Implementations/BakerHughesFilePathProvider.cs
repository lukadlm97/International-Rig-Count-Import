using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Io;
using Homework.Enverus.InternationalRigCountImport.Core.Configurations;
using Homework.Enverus.InternationalRigCountImport.Core.Exceptions;
using Homework.Enverus.InternationalRigCountImport.Core.Fetchers.Contracts;
using Homework.Enverus.Shared.Logging.Contracts;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Homework.Enverus.InternationalRigCountImport.Core.Fetchers.Implementations
{
    public class BakerHughesFilePathProvider : IFilePathProvider
    {
        private readonly ExternalDataSources _externalDataSource;
        private readonly IHighPerformanceLogger _logger;

        public BakerHughesFilePathProvider(IOptions<ExternalDataSources> options,
                                            IHighPerformanceLogger logger)
        {
            _externalDataSource = options.Value;
            _logger = logger;
        }
        public async Task<string?> GetFilePath(CancellationToken cancellationToken)
        {
            try
            {
                var exactUrl = string
                    .Format("{0}/{1}", _externalDataSource.BaseUrl,
                        _externalDataSource.RetrieveFileUrl);
                var requester = new DefaultHttpRequester();
                requester.Headers["User-Agent"] = _externalDataSource.UserAgent;
                var config = Configuration.Default.With(requester).WithDefaultLoader();
                var context = BrowsingContext.New(config);

                IDocument document = await context
                    .OpenAsync(exactUrl, cancellationToken);
                var anchorElement = document.QuerySelector($"a[title='{_externalDataSource.RetrieveTagFile}']");

                if (anchorElement != null)
                {
                    var url = anchorElement.GetAttribute("href");
                    if (url != null)
                    {
                        if (url.StartsWith('/'))
                        {
                            return url.Substring(1);
                        }

                        throw new UnexpectedUrlFormatException("invalid url format:" + url);
                    }
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                _logger.Log("Error occurred on try to determinate source file URL on website...", ex, LogLevel.Error);
                return string.Empty;
            }
        }
    }
}
