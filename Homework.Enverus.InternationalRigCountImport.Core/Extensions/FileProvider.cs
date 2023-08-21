using Homework.Enverus.InternationalRigCountImport.Core.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace Homework.Enverus.InternationalRigCountImport.Core.Extensions
{
    public static class FileProvider
    {
        public static IServiceCollection ConfigureFileProvider(this IServiceCollection services, 
            IConfiguration configuration, 
            string hostEnvironment)
        {
            /*
            var projectPath = string.Empty;
            var excelRootPath = string.Empty;

            if (!hostEnvironment.Equals("Staging", StringComparison.OrdinalIgnoreCase))
            {
                projectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
                excelRootPath = Path.Combine(projectPath, ExcelFileConstants.ExcelFileRootDirectory);
                services.Configure<ExcelDirectorySettings>(options => options.Root = excelRootPath);

                return services;
            }
            else
            {
                projectPath = configuration.GetSection(nameof(ExcelDirectorySettings)+":Root").Value;
                excelRootPath = Path.Combine(projectPath, ExcelFileConstants.ExcelFileRootDirectory);
            }*/
            var projectPath = configuration.GetSection(nameof(ExcelDirectorySettings) + ":Root").Value;
            var excelRootPath = Path.Combine(projectPath, ExcelFileConstants.ExcelFileRootDirectory);

            services.Configure<ExcelDirectorySettings>(options => options.Root = excelRootPath);

            return services;
        }
    }
}
