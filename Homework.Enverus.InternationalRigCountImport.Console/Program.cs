using Homework.Enverus.InternationalRigCountImport.Console.Services.Implementation;
using Homework.Enverus.InternationalRigCountImport.Core;
using Homework.Enverus.Shared.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args); 
var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
builder.Configuration
    .AddJsonFile($"appsettings.json", true, true)
    .AddJsonFile($"appsettings.{environment}.json", true, true)
    .AddEnvironmentVariables();

builder.Services.ConfigureLoggingServices(builder.Logging);

builder.Services.ConfigureApplicationServices(builder.Configuration, environment);

builder.Services.AddSingleton<InternationalRigCountBackgroundService>();
builder.Services.AddHostedService<InternationalRigCountBackgroundService>();

using IHost host = builder.Build();

await host.RunAsync();

