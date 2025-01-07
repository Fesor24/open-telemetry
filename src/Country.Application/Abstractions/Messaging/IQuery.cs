using Country.Domain.Abstractions;
using MediatR;

namespace Country.Application.Abstractions.Messaging
{
    public interface IQuery<TResponse> : IRequest<Result<TResponse>>;
}
