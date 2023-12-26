namespace Records.Shared.IntegrationEvents.Abstractions;

/// <summary>
/// Defines an integration event.
/// </summary>
public interface IIntegrationEvent
{
    #region Properties

    /// <summary>When the event occurred (UTC).</summary>
    public DateTime EventOccurredOn { get; }

    #endregion
}
