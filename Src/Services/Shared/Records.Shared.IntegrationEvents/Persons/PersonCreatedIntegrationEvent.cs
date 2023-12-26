using System;

namespace Records.Shared.IntegrationEvents.Persons;

/// <summary>
/// Represents an integration event to indicate that a Person (from Persons microservice)
/// was created.
/// </summary>
public sealed class PersonCreatedIntegrationEvent : IntegrationEvent
{
    #region Properties

    /// <summary>The Person Id.</summary>
    public Guid Id { get; init; }

    /// <summary>The Person name.</summary>
    public string? Name { get; init; }

    #endregion
}
