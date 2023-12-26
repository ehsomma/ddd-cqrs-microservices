namespace Records.Shared.Domain.Events;

/// <summary>
/// Represents the interface for an event that is raised within the domain.
/// </summary>
public interface IDomainEvent
{
    #region Properties

    /// <summary>When the event occurred (UTC).</summary>
    public DateTime EventOccurredOnUtc { get; }

    /// <summary>The aggregate id that the message belongs.</summary>
    /// <remarks>
    /// It is used to create MessageMetadata (and set the ContentId) to avoid navigating through
    /// message content to get the id of the aggregate.
    /// NOTE: Id could be string, guid, int, etc., so here we use string and you must cast your id
    /// to string.
    /// </remarks>
    public string AggregateId { get; }

    #endregion
}
