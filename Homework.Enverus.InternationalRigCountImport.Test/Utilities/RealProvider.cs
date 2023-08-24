using Homework.Enverus.InternationalRigCountImport.Core.Configurations;
using Homework.Enverus.InternationalRigCountImport.Core.Fetchers.Contracts;
using Homework.Enverus.InternationalRigCountImport.Core.Fetchers.Implementations;
using Homework.Enverus.InternationalRigCountImport.Core.Repositories.Contracts;
using Homework.Enverus.InternationalRigCountImport.Core.Services.Contracts;
using Homework.Enverus.InternationalRigCountImport.Core.Services.Implementations;
using Homework.Enverus.InternationalRigCountImport.Test.Mocks;
using Homework.Enverus.Shared.Logging.Contracts;
using Microsoft.Extensions.Options;

namespace Homework.Enverus.InternationalRigCountImport.Test.Utilities
{
    public static class RealProvider
    {
        public static ICsvService GetCsvService(IHighPerformanceLogger logger, IFileRepository repository)
        {
            var exporterSettings = Options.Create(new Exporter()
            {
                DataSourceSettings = new DataSourceSettings()
                {
                    ExcelWorkbookSettings = new ExcelWorkbookSettings()
                    {

                        Worksheet = "Worldwide_Rigcount",
                        StartRow = 7,
                        StartColumn = 2,
                        EndRow = 740,
                        EndColumn = 11,
                        RowsPerYear = 2,
                        Years = 1
                    }
                },
                ExportDestinationSettings = new ExportDestinationSettings()
                {
                    CsvSettings = new CsvSettings()
                    {
                        Delimiter = ",",
                        FileName = "output.csv"
                    }
                }
            });
            return new CsvService(logger, repository, exporterSettings);
        } 
        public static ICsvService GetCsvService(IHighPerformanceLogger logger, IFileRepository repository, ExportDestinationSettings exportDestinationSettings)
        {
            var exporterSettings = Options.Create(new Exporter()
            {
                DataSourceSettings = new DataSourceSettings()
                {
                    ExcelWorkbookSettings = new ExcelWorkbookSettings()
                    {

                        Worksheet = "Worldwide_Rigcount",
                        StartRow = 7,
                        StartColumn = 2,
                        EndRow = 740,
                        EndColumn = 11,
                        RowsPerYear = 2,
                        Years = 1
                    }
                },
                ExportDestinationSettings = exportDestinationSettings
            });
            return new CsvService(logger, repository, exporterSettings);
        }
        public static IExcelService GetExcelService(IHighPerformanceLogger logger, IFileRepository repository)
        {
            var exporterSettings = Options.Create(new Exporter()
            {
                DataSourceSettings = new DataSourceSettings()
                {
                    ExcelWorkbookSettings = new ExcelWorkbookSettings()
                    {

                        Worksheet = "Worldwide_Rigcount",
                        StartRow = 7,
                        StartColumn = 2,
                        EndRow = 740,
                        EndColumn = 11,
                        RowsPerYear = 2,
                        Years = 1
                    }
                },
                ExportDestinationSettings = new ExportDestinationSettings()
                {
                    CsvSettings = new CsvSettings()
                    {
                        Delimiter = ",",
                        FileName = "output.csv"
                    }
                }
            });
            var excelDirSettings = Options.Create(new ExcelDirectorySettings()
            {
                OriginalExcelRoot = ""
            });
            return new ExcelService(logger, exporterSettings, excelDirSettings, repository);
        }

        public static IFileProvider GetPathProvider(IHighPerformanceLogger highPerformanceLogger, ExternalDataSources externalDataSources)
        {
            var factory = MockHttpClientFactory.GetHttpClientFactoryWithDataMock();
            var persistenceOptions = Options.Create(externalDataSources);
            IFileProvider fileProvider = new BakerHughesFileProvider(factory.Object, persistenceOptions,
                highPerformanceLogger);

            return fileProvider;
        }
        public static IFileProvider GetPathProvider(IHighPerformanceLogger highPerformanceLogger)
        {
            var factory = MockHttpClientFactory.GetHttpClientFactoryWithDataMock();
            var persistenceOptions = Options.Create(new ExternalDataSources()
            {
                StaticFileUrl = "static-files/7240366e-61cc-4acb-89bf-86dc1a0dffe8",
                RetrieveTagFile = "Worldwide Rig Count Jul 2023.xlsx",
                UserAgent = "PostmanRuntime/7.32.3"
            });
            IFileProvider fileProvider = new BakerHughesFileProvider(factory.Object, persistenceOptions,
                highPerformanceLogger);

            return fileProvider;
        }
    }
}

