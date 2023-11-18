using Shopping.Aggregator.Services;
using Serilog;
using Common.Logging;
using Shopping.Aggregator.Helpers;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog(SeriLogger.Configure);

builder.Services.AddTransient<LoggingDelegatingHandler>();

// Add services to the container.
builder.Services.AddHttpClient<ICatalogService, CatalogService>(c =>
{
    c.BaseAddress = new Uri(builder.Configuration["ApiSettings:CatalogUrl"]);
})
.AddHttpMessageHandler<LoggingDelegatingHandler>()
.AddPolicyHandler(PollyHelper.GetRetryPolicy())
.AddPolicyHandler(PollyHelper.GetCircuitBreakerPolicy());


builder.Services.AddHttpClient<IBasketService, BasketService>(c =>
{
    c.BaseAddress = new Uri(builder.Configuration["ApiSettings:BasketUrl"]);
}).AddHttpMessageHandler<LoggingDelegatingHandler>()
.AddPolicyHandler(PollyHelper.GetRetryPolicy())
.AddPolicyHandler(PollyHelper.GetCircuitBreakerPolicy());

builder.Services.AddHttpClient<IOrderService, OrderService>(c =>
{
    c.BaseAddress = new Uri(builder.Configuration["ApiSettings:OrderingUrl"]);
}).AddHttpMessageHandler<LoggingDelegatingHandler>()
.AddPolicyHandler(PollyHelper.GetRetryPolicy())
.AddPolicyHandler(PollyHelper.GetCircuitBreakerPolicy());

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Shopping.Aggregator",
        Version = "v1"
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
