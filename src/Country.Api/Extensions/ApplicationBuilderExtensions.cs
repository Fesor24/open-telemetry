using System.Text.Json;
using Country.Api.Middleware;
using Country.Domain.Operators;
using Country.Infrastructure;
using CountryDomain = Country.Domain.Countries.Country;

namespace Country.Api.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static async Task SeedData(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();

            using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "";

            if (!context.Set<CountryDomain>().Any())
            {
                string countryContent, operatorContent;

                if (env == "Docker")
                {
                    countryContent = File.ReadAllText("country.json");
                    operatorContent = File.ReadAllText("operator.json");
                }
                else
                {
                    countryContent = File.ReadAllText("../Country.Api/country.json");
                    operatorContent = File.ReadAllText("../Country.Api/operator.json");
                }

                var data = JsonSerializer.Deserialize<List<CountrySeedData>>(countryContent, ReadOptions);
                var operators = JsonSerializer.Deserialize<List<OperatorSeedData>>(operatorContent, ReadOptions);

                List<CountryDomain> countries = [];
                List<Operator> countryOperators = [];

                foreach (var item in data!)
                {
                    CountryDomain country = new(item.Name,
                        new Domain.Countries.CountryCode(item.Code),
                        new Domain.Countries.IsoCode(item.Iso));

                    var countryOperators_ = operators!.Where(op => op.CountryIso == country.IsoCode.Value)
                        .Select(op => new Operator(op.Operator, op.Code, country))
                        .ToList();

                    countries.Add(country);
                    countryOperators.AddRange(countryOperators_);
                }

                await context.Set<CountryDomain>().AddRangeAsync(countries);

                await context.SaveChangesAsync();

                await context.Set<Operator>().AddRangeAsync(countryOperators);

                await context.SaveChangesAsync();
            }
        }

        public static void UseGlobalExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<GlobalExceptionHandler>();
        }

        internal static JsonSerializerOptions ReadOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    internal sealed class CountrySeedData
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Iso { get; set; }
    }

    internal sealed class OperatorSeedData
    {
        public string CountryIso { get; set; }
        public string Operator { get; set; }
        public string Code { get; set; }
    }
}
