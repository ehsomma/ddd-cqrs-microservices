using System;

namespace Records.Shared.IntegrationEvents.Persons;

/// <summary>
/// Represents an integration event to indicate that a Person (from Persons microservice)
/// was updated.
/// </summary>
public sealed class PersonUpdatedIntegrationEvent : IntegrationEvent
{
    /// <summary>The Person Id.</summary>
    public Guid Id { get; init; }

    /// <summary>The Person Name.</summary>
    public string? Name { get; init; }
}
