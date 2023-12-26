namespace Records.Shared.Infra.MessageBroker.Abstractions;

/// <summary>
/// Defines the EventBus actions.
/// </summary>
public interface IEventBus
{
    #region Public methods

    /// <summary>
    /// Publishes a message to all subscribed consumers for the message type as specified
    /// by the generic parameter. The second parameter allows the caller to customize the
    /// outgoing publish context and set things like headers on the message.
    /// </summary>
    /// <typeparam name="T">The type of the message.</typeparam>
    /// <param name="message">The messages to be published.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task PublishAsync<T>(T message, CancellationToken cancellationToken = default)
        where T : class;

    /// <summary>
    /// Publishes an object as a message, using the specified message type. If the object cannot be cast
    /// to the specified <paramref name="message"/> type, an exception will be thrown.
    /// NOTE: Used to publish events that implements an interface, not an especific T (like in the
    /// forEach of domain events list that implements IDomainEvent.
    /// </summary>
    /// <param name="message">The message object.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task PublishAsync(object message, CancellationToken cancellationToken = default);

    /// <summary>
    /// Send a message.
    /// </summary>
    /// <typeparam name="T">The message type.</typeparam>
    /// <param name="address">The endpoint address.</param>
    /// <param name="message">The message.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task SendAsync<T>(Uri address, T message, CancellationToken cancellationToken = default)
        where T : class;

    #endregion
}
