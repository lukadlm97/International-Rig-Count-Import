
using System.Text.Json;
using Homework.Enverus.InternationalRigCountImport.Core.Configurations;
using Homework.Enverus.InternationalRigCountImport.Core.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Homework.Enverus.InternationalRigCountImport.Core.Extensions
{
    public static class HttpClient
    {
        public static IServiceCollection ConfigureHttpClient(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
           
            serviceCollection.AddHttpClient(Constants.HttpClientName,
                builder =>
                {
                    builder.BaseAddress = new Uri(configuration.GetSection(nameof(ExternalDataSources)+ ":BaseUrl").Value);
                    builder.DefaultRequestHeaders.Add("User-Agent", configuration.GetSection(nameof(ExternalDataSources) + ":UserAgent").Value);
                    builder.DefaultRequestHeaders.Add("Accept", "*/*");
                    builder.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
                });

            return serviceCollection;
        }
    }
}
