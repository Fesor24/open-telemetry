using Country.Application.Abstractions.Cache;
using Country.Domain.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Country.Application.Abstractions.Behavior
{
    internal sealed class QueryCachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICachedQuery
        where TResponse : Result
    {
        private readonly ICacheService _cacheService;
        private readonly ILogger<QueryCachingBehavior<TRequest, TResponse>> _logger;

        public QueryCachingBehavior(ICacheService cacheService, ILogger<QueryCachingBehavior<TRequest, TResponse>> logger)
        {
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            string name = typeof(TRequest).Name;

            var result = _cacheService.Get<TResponse>(request.Key);

            if(result is not null)
            {
                _logger.LogInformation("Cache hit for {Query}", name);

                return result;
            }

            _logger.LogInformation("Cache miss for {Query}", name);

            var response = await next();

            if (response.IsSuccess)
            {
                _cacheService.Set(request.Key, response, request.Expiration);
            }

            return response;
        }
    }
}
