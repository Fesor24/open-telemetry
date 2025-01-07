using Country.Api.Endpoints.Countries;
using Country.Api.Extensions;
using Country.Application;
using Country.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddInfrastructureServices()
    .AddApplicationServices();

builder.Services.AddOpenApi();

var app = builder.Build();

app.UseGlobalExceptionMiddleware();

await app.SeedData();

CountryEndpoint.RegisterEndpoint(app);

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseSwaggerUI(opts => opts.SwaggerEndpoint("/openapi/v1.json", "Endpoints"));

app.Run();
