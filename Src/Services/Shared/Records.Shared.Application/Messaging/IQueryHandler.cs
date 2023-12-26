using MediatR;

namespace Records.Shared.Application.Messaging;

/// <summary>
/// Represents the handler for an <see cref="IQuery{TResponse}"/>.
/// </summary>
/// <typeparam name="TQuery">The query type.</typeparam>
/// <typeparam name="TResponse">The query response type.</typeparam>
public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{
}
