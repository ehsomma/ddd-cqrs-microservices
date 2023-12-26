using System.Reflection;

namespace Records.Shared.Messaging;

/// <summary>
/// Represents the metadata of a <see cref="Message"/>.
/// </summary>
public class MessageMetadata : IMessageMetadata
{
    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="MessageMetadata"/> class.
    /// </summary>
    /// <remarks>
    /// NOTE: MassTransit uses this constructor when deserializing (when trying to consume a Rabbit
    /// message), generating new guids, but then initializing them with the original data sent in the
    /// Message.Metadata (requires that the properties of this class always be get; init;).
    /// </remarks>
    public MessageMetadata()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MessageMetadata"/> class.
    /// </summary>
    /// <param name="contentId">The id of the object in the content.</param>
    public MessageMetadata(string contentId)
        : this(Guid.Empty, Guid.Empty, string.Empty, contentId)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MessageMetadata"/> class.
    /// </summary>
    /// <param name="causationMetadata">The <see cref="MessageMetadata"/> of the causation <see cref="Message"/>.</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Don't know how to do it :D")]
    public MessageMetadata(MessageMetadata causationMetadata)
        : this(causationMetadata.CorrelationId, causationMetadata.MessageId, causationMetadata.Host, causationMetadata.ContentId)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MessageMetadata"/> class.
    /// </summary>
    /// <param name="correlationId">The id of the first message that caused the occurrence of this message.</param>
    /// <param name="causationId">The id of the message that caused the occurrence of this message.</param>
    /// <param name="host">The name of the assembly from which it was created.</param>
    /// <param name="contentId">The id of the object in the content.</param>
    private MessageMetadata(Guid correlationId, Guid causationId, string host, string contentId)
    {
        MessageId = Guid.NewGuid();

        if (correlationId == Guid.Empty)
        {
            correlationId = MessageId;
        }

        if (causationId == Guid.Empty)
        {
            causationId = correlationId;
        }

        if (string.IsNullOrWhiteSpace(host))
        {
            ////Host = Assembly.GetEntryAssembly()?.FullName ?? "(unresolved)";
            ////Host = Assembly.GetExecutingAssembly()?.FullName ?? "(unresolved)";
            ////host = Assembly.GetEntryAssembly()?.FullName ?? "(unresolved)";
            host = Assembly.GetEntryAssembly()?.GetName().Name ?? "(unresolved)";
        }

        CorrelationId = correlationId;
        CausationId = causationId;
        CreatedOnUtc = DateTime.UtcNow;
        PublishedOnUtc = null;
        Host = host;
        Version = "1";
        ContentId = contentId;
    }

    #endregion

    #region Properties

    /// <inheritdoc />
    public Guid MessageId { get; init; }

    /// <inheritdoc />
    public Guid CorrelationId { get; init; }

    /// <inheritdoc />
    public Guid CausationId { get; init; }

    /// <inheritdoc />
    public DateTime CreatedOnUtc { get; init; }

    /// <inheritdoc />
    /// <remarks>
    /// NOTE: Currently this property is not used since after publishing the message it is deleted
    /// from the Outbox.
    /// </remarks>
    public DateTime? PublishedOnUtc { get; init; }

    /// <inheritdoc />
    public string Host { get; init; } = string.Empty;

    /// <inheritdoc />
    public string Version { get; init; } = string.Empty;

    /// <inheritdoc />
    /// <remarks>
    /// It is just to check obsolete messages for a specific aggregate in scenarios where the message
    /// will cause some updates to it.
    /// NOTE: Id could be string, guid, int, etc., so here we use string and you must cast your id
    /// to string.
    /// </remarks>
    public string ContentId { get; init; } = string.Empty;

    /*
    IMPORTANT: If you add one more property in MessageMetadata, you must add it to IMessageMetadata
    and implement it in OutboxMessage since the Metadata properties are saved flattened in the
    OutboxMessage table. Otherwise they will be empty when obtaining the Metadata from the Outbox at
    the time of publishing the message. They must also be considered in OutboxRepository.SaveAsync(),
    OutboxMapper.Map() and Message.Build().
    */

    #endregion
}
