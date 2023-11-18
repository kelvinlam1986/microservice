using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Cache.CacheManager;
using Serilog;
using Common.Logging;
using System.Diagnostics;
using Ocelot.Values;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry;
using OpenTelemetry.Exporter;

Activity.DefaultIdFormat = ActivityIdFormat.W3C;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.Configure(options =>
{
    options.ActivityTrackingOptions = ActivityTrackingOptions.TraceId | ActivityTrackingOptions.SpanId;
});


builder.Host.ConfigureAppConfiguration((context, cfg) =>
{
    cfg.AddJsonFile($"ocelot.{context.HostingEnvironment.EnvironmentName}.json", true, true);
});

builder.Host.UseSerilog(SeriLogger.Configure);

//builder.Host.ConfigureLogging(logging =>
//{
//    logging.ClearProviders();
//    logging.AddConsole();
//    logging.AddDebug();
//});

builder.Services.AddOcelot()
    .AddCacheManager(settings => settings.WithDictionaryHandle());

    builder.Services.AddOpenTelemetryTracing((builder) =>
    {
        builder
            .AddAspNetCoreInstrumentation()
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("OcelotApiGw"))
            .AddHttpClientInstrumentation()
            .AddSource(nameof(IOcelotBuilder))
            .AddJaegerExporter(options =>
            {
                options.AgentHost = "localhost";
                options.AgentPort = 6831;
                options.ExportProcessorType = ExportProcessorType.Simple;
            })
            .AddConsoleExporter(options =>
            {
                options.Targets = ConsoleExporterOutputTargets.Console;
            });
    });

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

await app.UseOcelot();

await app.RunAsync();
