namespace Records.Shared.Domain.Events;

/// <summary>
/// Represents an event that is raised within the domain.
/// </summary>
public class DomainEvent : IDomainEvent
{
    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="DomainEvent"/> class.
    /// </summary>
    /// <param name="aggregateId"><inheritdoc cref="AggregateId" path="/summary"/></param>
    public DomainEvent(string aggregateId)
    {
        AggregateId = aggregateId;
        EventOccurredOnUtc = DateTime.UtcNow;
    }

    #endregion

    /// <inheritdoc />
    public DateTime EventOccurredOnUtc { get; }

    /// <inheritdoc />
    public string AggregateId { get; }
}
