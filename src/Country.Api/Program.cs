using System.Diagnostics;
using Country.Api.Endpoints.Countries;
using Country.Api.Extensions;
using Country.Application;
using Country.Infrastructure;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddInfrastructureServices()
    .AddApplicationServices();

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

await app.SeedData();

CountryEndpoint.RegisterEndpoint(app);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// for .net 9...
//app.UseSwaggerUI(opts => opts.SwaggerEndpoint("/openapi/v1.json", "Endpoints"));

app.Run();
