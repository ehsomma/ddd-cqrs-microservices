#region Usings

using System;
using System.Threading.Tasks;

#endregion

namespace Records.Shared.Infra.Idempotence.Abstractions;

/// <summary>
/// Defines a class to manage and check obsoletes messages.
/// </summary>
public interface IObsoleteMessageService
{
    #region Public methods

    /// <summary>
    /// Checks id the message with the specified <paramref name="contentId"/> is obsolete.
    /// </summary>
    /// <param name="messageTypeName">The message type name.</param>
    /// <param name="createdOnUtc">DateTime (UTC) when the message was created.</param>
    /// <param name="contentId">The id of the content object of the message.</param>
    /// <returns>True, if the message is obsolete. Otherwise, false.</returns>
    public Task<bool> IsNotObsolete(string messageTypeName, DateTime createdOnUtc, string contentId);

    /// <summary>
    /// Saves a key to check if obsolete later.
    /// </summary>
    /// <param name="messageTypeName">The message type name.</param>
    /// <param name="createdOnUtc">DateTime (UTC) when the message was created.</param>
    /// <param name="contentId">The id of the content object of the message.</param>
    public void Save(string messageTypeName, DateTime createdOnUtc, string contentId);

    /// <summary>
    /// Saves a key to check if obsolete later.
    /// </summary>
    /// <param name="messageTypeName">The message type name.</param>
    /// <param name="createdOnUtc">DateTime (UTC) when the message was created.</param>
    /// <param name="contentId">The id of the content object of the message.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task SaveAsync(string messageTypeName, DateTime createdOnUtc, string contentId);

    #endregion
}
