using System.Threading.Tasks;

namespace Records.Shared.Infra.Idempotence.Abstractions;

/// <summary>
/// Defines a class to manage and check idempotentcy in messages.
/// </summary>
public interface IIdempotentMessageService
{
    #region Public methods

    /// <summary>
    /// Checks if the specified message (bay name/id) has been processed or not.
    /// </summary>
    /// <param name="messageId">The message id.</param>
    /// <param name="messageTypeName">The message type name.</param>
    /// <returns>True, if the message has already processed.</returns>
    public Task<bool> HasBeenProcessed(string messageId, string messageTypeName);

    /// <summary>
    /// Saves a key to check for idempotency later.
    /// </summary>
    /// <param name="messageId">The message id.</param>
    /// <param name="messageTypeName">The message type name.</param>
    public void Save(string messageId, string messageTypeName);

    /// <summary>
    /// Saves a key to check for idempotency later.
    /// </summary>
    /// <param name="messageId">The message id.</param>
    /// <param name="messageTypeName">The message type name.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task SaveAsync(string messageId, string messageTypeName);

    #endregion
}
