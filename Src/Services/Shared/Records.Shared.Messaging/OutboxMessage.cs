namespace Records.Shared.Messaging;

/// <summary>
/// Represents the <see cref="OutboxMessage"/> to implement the Outbox pattern for a <see cref="Message"/>.
/// </summary>
public class OutboxMessage : IMessageMetadata
{
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
    public DateTime? PublishedOnUtc { get; init; }

    /// <inheritdoc />
    public string ContentId { get; init; } = string.Empty;

    /// <inheritdoc />
    public string Host { get; init; } = string.Empty;

    /// <inheritdoc />
    public string Version { get; init; } = string.Empty;

    // End Metadata properties.

    /// <summary>Saved date.</summary>
    public DateTime SavedOn { get; set; }

    /// <summary>The contend serialized.</summary>
    public string Content { get; init; } = string.Empty;

    /// <summary>The content type to know how to deserialize it.</summary>
    public string ContentType { get; init; } = string.Empty;

    /// <summary>The error description if there is an error when the outbox try to publish the message.</summary>
    public string? Error { get; set; }

    /// <summary>The retries count.</summary>
    public int Retries { get; set; }

    #endregion
}
