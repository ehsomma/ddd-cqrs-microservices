namespace Records.Shared.Messaging;

/// <summary>
/// Defines a mapper to map <see cref="Message{TContent}"/> to <see cref="OutboxMessage"/>.
/// </summary>
public interface IOutboxMapper
{
    #region Public methods

    /// <summary>
    /// Maps <see cref="Message{TContent}"/> to <see cref="OutboxMessage"/>.
    /// </summary>
    /// <typeparam name="TContent">The type of the content of the message.</typeparam>
    /// <param name="message">The message to map.</param>
    /// <returns>An <see cref="OutboxMessage"/>.</returns>
    OutboxMessage Map<TContent>(Message<TContent> message);

    #endregion
}
