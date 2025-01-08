using Country.Application.Abstractions.Cache;
using Country.Domain.Countries;
using Country.Infrastructure.Cache;
using Country.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Country.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            AddPersistence(services);
            AddCaching(services);

            return services;
        }

        private static void AddPersistence(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("Country"));

            services.AddScoped<ICountryRepository, CountryRepository>();
        }

        private static void AddCaching(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddScoped<ICacheService, CacheService>();
        }
    }
}
