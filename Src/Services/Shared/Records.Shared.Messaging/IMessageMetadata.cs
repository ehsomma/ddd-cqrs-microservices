namespace Records.Shared.Messaging;

/// <summary>
/// Defines the <see cref="Message"/> metadata.
/// </summary>
public interface IMessageMetadata
{
    #region Properties

    /// <summary>The unique id for this message.</summary>
    Guid MessageId { get; init; }

    /// <summary>The id of the first message that caused the occurrence of this message.</summary>
    Guid CorrelationId { get; init; }

    /// <summary>The id of the message that caused the occurrence of this message.</summary>
    Guid CausationId { get; init; }

    /// <summary>Created date (UTC).</summary>
    DateTime CreatedOnUtc { get; init; }

    /// <summary>Published date (UTC).</summary>
    DateTime? PublishedOnUtc { get; init; }

    /// <summary>The id of the object in the content (e.g: the aggregate id).</summary>
    /// <remarks>It is duplicated here to ovoid deserialize the message content to get the id (e.g. it is used to check if the message is "obsolete").</remarks>
    string ContentId { get; init; }

    /// <summary>The name of the assembly from which it was created.</summary>
    string Host { get; init; }

    /// <summary>The version of the Message structure for deserialization purpose.</summary>
    string Version { get; init; }

    #endregion
}
