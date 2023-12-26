using MediatR;

namespace Records.Shared.Application.Messaging;

/// <summary>
/// Defines the base command handler for an <see cref="ICommand{TResponse}"/> with a TMessage.
/// </summary>
/// <typeparam name="TCommand">The command type to handle.</typeparam>
/// <typeparam name="TResponse">The command response type.</typeparam>
public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
}

/// <summary>
/// Defines the base command handler for a <see cref="ICommand"/> with void response.
/// </summary>
/// <typeparam name="TCommand">The command type.</typeparam>
public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand>
    where TCommand : ICommand
{
}
