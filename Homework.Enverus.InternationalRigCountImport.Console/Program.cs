using Homework.Enverus.InternationalRigCountImport.Console.Services.Implementation;
using Homework.Enverus.InternationalRigCountImport.Core.Configurations;
using Homework.Enverus.InternationalRigCountImport.Core.Extensions;
using Homework.Enverus.InternationalRigCountImport.Core.Fetchers.Contracts;
using Homework.Enverus.InternationalRigCountImport.Core.Fetchers.Implementations;
using Homework.Enverus.InternationalRigCountImport.Core.Repositories.Contracts;
using Homework.Enverus.InternationalRigCountImport.Core.Repositories.Implementations;
using Homework.Enverus.InternationalRigCountImport.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args); 
var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
builder.Configuration
    .AddJsonFile($"appsettings.json", true, true)
    .AddJsonFile($"appsettings.{environment}.json", true, true)
    .AddEnvironmentVariables();
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.Configure<ExternalDataSources>(builder.Configuration.GetSection(nameof(ExternalDataSources)));
builder.Services.Configure<Exporter>(builder.Configuration.GetSection(nameof(Exporter)));
builder.Services.Configure<AdvancedSettings>(builder.Configuration.GetSection(nameof(AdvancedSettings)));

builder.Services.ConfigureHttpClient(builder.Configuration);
builder.Services.ConfigureAdvancedFileProvider(builder.Configuration, environment);

builder.Services.AddSingleton<IFilePathProvider, BakerHughesFilePathProvider>();
builder.Services.AddSingleton<IFileProvider, BakerHughesFileProvider>();
builder.Services.AddSingleton<IExcelFileRepository, ExcelFileRepository>();
builder.Services.AddSingleton<ICsvRepository, CsvRepository>();
builder.Services.AddSingleton<IRigCountExporter, RigCountExporter>();
builder.Services.AddSingleton<InternationalRigCountBackgroundService>();
builder.Services.AddHostedService<InternationalRigCountBackgroundService>();

using IHost host = builder.Build();

await host.RunAsync();

