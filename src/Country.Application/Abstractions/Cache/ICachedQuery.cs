using Country.Application.Abstractions.Messaging;

namespace Country.Application.Abstractions.Cache
{
    public interface ICachedQuery<TResponse> : IQuery<TResponse>, ICachedQuery;

    public interface ICachedQuery
    {
        string Key { get; }
        TimeSpan Expiration { get; }
    }
}
