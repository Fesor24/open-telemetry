using System.Diagnostics;
using System.Reflection;
using Country.Api.Endpoints.Countries;
using Country.Api.Extensions;
using Country.Application;
using Country.Application.Abstractions.Diagnostics;
using Country.Infrastructure;
using Microsoft.AspNetCore.Http.Features;
using Npgsql;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddInfrastructureServices()
    .AddApplicationServices();

//string redisConnectionString = "";

//IConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect(redisConnectionString);

//builder.Services.AddSingleton(connectionMultiplexer);

//builder.Services.AddStackExchangeRedisCache(options =>
//    options.ConnectionMultiplexerFactory = () => Task.FromResult(connectionMultiplexer));

//builder.Services.AddSingleton<IConnectionMultiplexer>(instance =>
//{
//    var configuration = ConfigurationOptions.Parse(redisConnectionString);

//    return ConnectionMultiplexer.Connect(configuration);
//});


builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource
        .AddService("Country.Api", serviceVersion: Assembly.GetExecutingAssembly().GetName().Version!.ToString(),
        serviceNamespace: "open-telemetry-course")
    .AddAttributes(new[]
    {
        new KeyValuePair<string, object>("service.version", Assembly.GetExecutingAssembly().GetName().Version!.ToString())
    })// specify custom attributes...
    )// configure resource...
    .WithTracing(trc => 
        trc.AddAspNetCoreInstrumentation()// will take care of the activities...
        //.AddNpgsql()
        /*.AddRedisInstrumentation()*/ // bcos of the singleton registered above for the connction multiplexer, it will be used here also...
        .AddConsoleExporter()
        .AddOtlpExporter(options =>
            options.Endpoint = new Uri("http://jaeger:4317"))// this can find the receiver of the data running on a mcahine,we can specify an endpoint also, where we want to push it to...
        )
    .WithMetrics(met => 
        met.AddMeter(ApplicationDiagnostics.Meter.Name) // specify the meter we want to track...the instruments created under this meter
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation() // metrics exposed by these packages...
        .AddMeter("Microsoft.AspNetCore.Hosting")
        .AddMeter("Microsoft.AspNetCore.Server.Kestrel")// we could also do this...metrics exposed by .NET
        .AddConsoleExporter()
        .AddPrometheusExporter()// prometheus will scrape data from the api container...
        );

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = ctx =>
    {
        ctx.ProblemDetails.Instance = $"{ctx.HttpContext.Request.Method} {ctx.HttpContext.Request.Path}";
        ctx.ProblemDetails.Extensions.TryAdd("requestId", ctx.HttpContext.TraceIdentifier);

        Activity? activity = ctx.HttpContext.Features.Get<IHttpActivityFeature>()?.Activity;

        ctx.ProblemDetails.Extensions.TryAdd("traceId", activity?.Id);
    };
});

var app = builder.Build();

app.UseGlobalExceptionMiddleware();

app.UseOpenTelemetryPrometheusScrapingEndpoint();// this would expose an endpoint prometheus can call to scrape data from

await app.SeedData();

CountryEndpoint.RegisterEndpoint(app);

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

// for .net 9...
//app.UseSwaggerUI(opts => opts.SwaggerEndpoint("/openapi/v1.json", "Endpoints"));

app.Run();
