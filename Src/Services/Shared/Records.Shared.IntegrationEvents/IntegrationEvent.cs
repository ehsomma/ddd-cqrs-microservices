#region Usings

using Records.Shared.IntegrationEvents.Abstractions;
using System;

#endregion

namespace Records.Shared.IntegrationEvents;

/// <summary>
/// Represents an integration event.
/// </summary>
public class IntegrationEvent : IIntegrationEvent
{
    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="IntegrationEvent"/> class.
    /// </summary>
    public IntegrationEvent()
    {
        EventOccurredOn = DateTime.UtcNow;
    }

    #endregion

    #region Properties

    /// <inheritdoc />
    public DateTime EventOccurredOn { get; }

    #endregion
}
