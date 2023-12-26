using System;

namespace Records.Shared.IntegrationEvents.Persons;

/// <summary>
/// Represents an integration event to indicate that a Person (from Persons microservice)
/// was deleted.
/// </summary>
public sealed class PersonDeletedIntegrationEvent : IntegrationEvent
{
    /// <summary>The Person Id.</summary>
    public Guid Id { get; init; }
}
