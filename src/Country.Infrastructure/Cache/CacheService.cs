using System.Text.Json;
using Country.Application.Abstractions.Cache;
using Microsoft.Extensions.Caching.Memory;

namespace Country.Infrastructure.Cache
{
    internal sealed class CacheService : ICacheService
    {
        private readonly IMemoryCache _cache;

        public CacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public TValue? Get<TValue>(string key, CancellationToken cancellationToken = default)
        {
            var cachedValue = _cache.Get<string>(key);

            return cachedValue is not null ?
                JsonSerializer.Deserialize<TValue>(cachedValue) : default;
        }

        public void Remove(string key, CancellationToken cancellationToken)
        {
            _cache.Remove(key);
        }

        public void Set<TValue>(string key, TValue value, TimeSpan expiration, CancellationToken cancellationToken = default)
        {
            string stringValue = JsonSerializer.Serialize(value);

            _cache.Set(key, stringValue, expiration);
        }
    }
}
