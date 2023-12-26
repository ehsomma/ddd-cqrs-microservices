using MediatR;

namespace Records.Shared.Application.Messaging;

/// <summary>
/// Represents a command with the data to be used in a command handler to execute a write operation
/// with a response of TResponse/>.
/// </summary>
/// <typeparam name="TResponse">The command response type.</typeparam>
public interface ICommand<out TResponse> : IRequest<TResponse>
{
}

/// <summary>
/// Represents a command with the data to be used in a command handler to execute a write operation
/// with void response.
/// </summary>
public interface ICommand : IRequest
{
}
