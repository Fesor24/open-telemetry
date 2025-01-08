using Country.Application.Abstractions.Behavior;
using Microsoft.Extensions.DependencyInjection;

namespace Country.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            AddMediator(services);
            return services;
        }

        public static void AddMediator(IServiceCollection services)
        {
            services.AddMediatR(options =>
            {
                options.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);

                options.AddOpenBehavior(typeof(QueryCachingBehavior<,>));
            });
        }
    }
}
