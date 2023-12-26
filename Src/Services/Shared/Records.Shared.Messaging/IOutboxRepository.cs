#region Usings

#endregion

namespace Records.Shared.Messaging;

/// <summary>
/// Define the Outbox repository.
/// </summary>
public interface IOutboxRepository
{
    #region Public methods

    /// <summary>
    /// Save the message into the Outbox repository.
    /// </summary>
    /// <typeparam name="TContent">The type of the content of the message.</typeparam>
    /// <param name="message">The <see cref="Message"/> to save.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task SaveAsync<TContent>(Message<TContent> message);

    /// <summary>
    /// Gets all the pending messages from the Outbox repository.
    /// </summary>
    /// <returns>The pending messages or null.</returns>
    public Task<IEnumerable<OutboxMessage>?> GetAllAsync();

    /// <summary>
    /// Remove the message from the Outbox repository.
    /// </summary>
    /// <param name="messageId">The id of the published message.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task PublishedAsync(Guid messageId);

    /// <summary>
    /// Marks the message as failed.
    /// </summary>
    /// <param name="messageId">The id of the message that failed at publishing.</param>
    /// <param name="error">The error description.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task FailedAsync(Guid messageId, string error);

    #endregion
}
