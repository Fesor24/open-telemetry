namespace Country.Application.Abstractions.Cache
{
    public interface ICacheService
    {
        TValue Get<TValue>(string key, CancellationToken cancellationToken = default);
        void Set<TValue>(string key, TValue value, TimeSpan expiration, CancellationToken cancellationToken = default);
        void Remove(string key, CancellationToken cancellationToken);
    }
}
