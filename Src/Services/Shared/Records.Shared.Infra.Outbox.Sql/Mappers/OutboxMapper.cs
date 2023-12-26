#region Usings

using System.Text.Json;
using Records.Shared.Messaging;

#endregion

namespace Records.Shared.Infra.Outbox.Sql.Mappers;

/// <summary>
/// Represent a mapper to map <see cref="Message{TContent}"/> to <see cref="OutboxMessage"/>.
/// </summary>
public class OutboxMapper : IOutboxMapper
{
    #region Public methods

    /// <summary>
    /// Maps <see cref="Message{TContent}"/> to <see cref="OutboxMessage"/>.
    /// </summary>
    /// <typeparam name="TContent">The type of the content of the message.</typeparam>
    /// <param name="message">The message to map.</param>
    /// <returns>An <see cref="OutboxMessage"/>.</returns>
    public OutboxMessage Map<TContent>(Message<TContent> message)
    {
        ArgumentNullException.ThrowIfNull(message);

        MessageMetadata metadata = message.Metadata;
        TContent content = message.Content;

        OutboxMessage outboxMessage = new ()
        {
            MessageId = metadata.MessageId,
            CorrelationId = metadata.CorrelationId,
            CausationId = metadata.CausationId,
            CreatedOnUtc = metadata.CreatedOnUtc,
            ContentId = metadata.ContentId,
            Host = metadata.Host,
            Version = metadata.Version,
            SavedOn = DateTime.UtcNow,
            ContentType = content!.GetType().AssemblyQualifiedName!,
            Content = JsonSerializer.Serialize(content),
        };

        return outboxMessage;
    }

    #endregion
}
