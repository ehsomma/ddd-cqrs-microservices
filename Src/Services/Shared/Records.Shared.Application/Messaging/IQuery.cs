using MediatR;

namespace Records.Shared.Application.Messaging;

/// <summary>
/// Represents the data to be used in a query handler to execute a read operation with a
/// response of TMessage.
/// </summary>
/// <typeparam name="TResponse">The query response type.</typeparam>
public interface IQuery<out TResponse> : IRequest<TResponse>
{
}
