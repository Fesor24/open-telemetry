using Country.Domain.Abstractions;
using MediatR;

namespace Country.Application.Abstractions.Messaging
{
    public interface IQueryHandler<TRequest, TResponse> : IRequestHandler<TRequest, Result<TResponse>>
        where TRequest : IQuery<TResponse>
    {
    }
}
