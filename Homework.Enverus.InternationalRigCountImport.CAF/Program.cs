using Homework.Enverus.InternationalRigCountImport.CAF.Scripts.Implementations;
using Homework.Enverus.InternationalRigCountImport.Core;
using Homework.Enverus.Shared.Logging;
using Microsoft.Extensions.Hosting;

var hostBuilder = Host.CreateDefaultBuilder(args)
    .ConfigureLogging((context, cfg) =>
    {
        cfg.ConfigureLoggingBuilder();
    })
    .ConfigureServices((hostingContext, services) =>
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        services.ConfigureLoggingServices();
        services.ConfigureApplicationServices(hostingContext.Configuration, environment);
    })
   ;


var app = ConsoleApp.CreateFromHostBuilder(hostBuilder, args);
app.AddCommands<Script>();
await app.RunAsync();


