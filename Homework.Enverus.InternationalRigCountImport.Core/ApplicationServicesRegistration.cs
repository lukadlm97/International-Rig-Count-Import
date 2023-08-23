using Homework.Enverus.InternationalRigCountImport.Core.Fetchers.Contracts;
using Homework.Enverus.InternationalRigCountImport.Core.Fetchers.Implementations;
using Homework.Enverus.InternationalRigCountImport.Core.Repositories.Contracts;
using Homework.Enverus.InternationalRigCountImport.Core.Repositories.Implementations;
using Homework.Enverus.InternationalRigCountImport.Core.Services.Contracts;
using Homework.Enverus.InternationalRigCountImport.Core.Services.Implementations;
using Microsoft.Extensions.DependencyInjection;
using Homework.Enverus.InternationalRigCountImport.Core.Extensions;
using Microsoft.Extensions.Configuration;
using Homework.Enverus.InternationalRigCountImport.Core.Configurations;

namespace Homework.Enverus.InternationalRigCountImport.Core
{
    public static class ApplicationServicesRegistration
    {
        public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services, 
                                                                            IConfiguration configuration, 
                                                                            string environment)
        {

            services.Configure<ExternalDataSources>(configuration.GetSection(nameof(ExternalDataSources)));
            services.Configure<Exporter>(configuration.GetSection(nameof(Exporter)));
            services.Configure<AdvancedSettings>(configuration.GetSection(nameof(AdvancedSettings)));

            services.ConfigureHttpClient(configuration);
            services.ConfigureAdvancedFileProvider(configuration, environment);

            services.AddSingleton<IFilePathProvider, BakerHughesFilePathProvider>();
            services.AddSingleton<IFileProvider, BakerHughesFileProvider>();
            services.AddSingleton<IRigCountExporter, RigCountExporter>();
            services.AddSingleton<IRigCountImporter, RigCountImporter>();
            services.AddSingleton<IExcelService, ExcelService>();
            services.AddSingleton<ICsvService, CsvService>();
            services.AddSingleton<IFileRepository, FileRepository>();


            return services;
        }


    }
}
