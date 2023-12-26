using MediatR;

namespace Records.Shared.Application.Messaging;

/// <summary>
/// Represents a domain event handler for a TMessage.
/// </summary>
/// <typeparam name="TMessage">The message type with a domain event.</typeparam>
public interface IDomainEventHandler<in TMessage> : INotificationHandler<TMessage>
    where TMessage : INotification
{
}
