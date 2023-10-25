using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Host.ConfigureAppConfiguration((context, cfg) =>
{
    cfg.AddJsonFile($"ocelot.{context.HostingEnvironment.EnvironmentName}.json", true, true);
});


builder.Host.ConfigureLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
    logging.AddDebug();
});

builder.Services.AddOcelot();
  
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

await app.UseOcelot();

await app.RunAsync();
