using System.Text.Json;
using Homework.Enverus.InternationalRigCountImport.Core.Configurations;
using Homework.Enverus.InternationalRigCountImport.Core.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Homework.Enverus.InternationalRigCountImport.Core.Extensions
{
    public static class FileProvider
    {
        /*
        public static IServiceCollection ConfigureFileProvider(this IServiceCollection services, 
            IConfiguration configuration, 
            string hostEnvironment)
        {
           

            return services;
        }
        */

        public static IServiceCollection ConfigureAdvancedFileProvider(this IServiceCollection services,
            IConfiguration configuration,
            string hostEnvironment)
        {
            var enabled = configuration.GetSection(nameof(AdvancedSettings)+ ":Enabled").Value;

            if (Cast(enabled))
            {
                var projectPath = configuration.GetSection(nameof(AdvancedSettings) + ":OriginalExcelLocation").Value;
                var excelRootPath = Path.Combine(projectPath, ExcelFileConstants.ExcelFileRootDirectory);

                services.Configure<ExcelDirectorySettings>(options =>
                {
                    options.OriginalExcelRoot = excelRootPath;
                    options.ExportCsvRoot = configuration.GetSection(nameof(AdvancedSettings) + ":CsvExportLocation").Value;
                });
            }
            

            return services;
        }

        static bool Cast(string value)
        {
            return value.ToLower().Equals("true", StringComparison.OrdinalIgnoreCase);
        }
    }
}
