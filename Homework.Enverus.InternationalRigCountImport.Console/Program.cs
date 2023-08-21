

using Homework.Enverus.InternationalRigCountImport.Console.Services.Implementation;
using Homework.Enverus.InternationalRigCountImport.Core.Configurations;
using Homework.Enverus.InternationalRigCountImport.Core.Extensions;
using Homework.Enverus.InternationalRigCountImport.Core.Fetchers.Contracts;
using Homework.Enverus.InternationalRigCountImport.Core.Fetchers.Implementations;
using Homework.Enverus.InternationalRigCountImport.Core.Models.Enums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.Configure<ScrapingEndpoints>(builder.Configuration.GetSection(nameof(ScrapingEndpoints)));

builder.Services.AddSingleton<IWebPageProcessor, WebPageProcessor>();
builder.Services.AddSingleton<InternationalRigCountBackgroundService>();
builder.Services.AddHostedService<InternationalRigCountBackgroundService>();
builder.Services.ConfigureHttpClient(builder.Configuration);

using IHost host = builder.Build();

await host.RunAsync();



async Task SaveFile(byte[] bytes)
{
    string filepath = Directory.GetCurrentDirectory();
    string filename = @"data.xlsx";
    string combinepath = filepath + filename;
    await File.WriteAllBytesAsync(combinepath, bytes, CancellationToken.None);
    Console.WriteLine("everything is fine");
}
/*

var url = @"https://bakerhughesrigcount.gcs-web.com/static-files/7240366e-61cc-4acb-89bf-86dc1a0dffe8";

HttpClient client = new HttpClient();
client.BaseAddress = new Uri(url);
client.DefaultRequestHeaders.Add("User-Agent", "PostmanRuntime/7.32.3");
client.DefaultRequestHeaders.Add("Accept", "");
client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");

var response = await client.GetAsync("", CancellationToken.None);
if (response.IsSuccessStatusCode)
{
    var bytes = await response.Content.ReadAsByteArrayAsync(CancellationToken.None);
    string filepath = Environment.CurrentDirectory;
    string filename = @"data.xlsx";
    string combinepath = filepath + filename;
    await File.WriteAllBytesAsync(combinepath, bytes, CancellationToken.None);
    Console.WriteLine("everything is fine");
    return;
}

Console.WriteLine("shits happend");
*/