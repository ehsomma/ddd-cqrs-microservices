namespace Records.Shared.IntegrationEvents.Persons;

/// <summary>
/// Represents an integration event to indicate that a Country (from Persons microservice) was created.
/// </summary>
/// <remarks>
/// [IMPORTANT] There are multiple CountryDeletedIntegrationEvent with different namespace:
///
/// • Countries.CountryCreatedIntegrationEvent published from Countries microservice.
/// • Persons.CountryCreatedIntegrationEvent published from Persons microservice.
/// </remarks>
public sealed class CountryCreatedIntegrationEvent : IntegrationEvent
{
    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CountryCreatedIntegrationEvent"/> class.
    /// </summary>
    /// <param name="iataCode">Country IATA code.</param>
    /// <param name="name">Country name.</param>
    public CountryCreatedIntegrationEvent(string iataCode, string name)
    {
        IataCode = iataCode;
        Name = name;
    }

    #endregion

    #region Properties

    /// <summary>Country IATA code.</summary>
    public string IataCode { get; }

    /// <summary>Country name.</summary>
    public string Name { get; }

    #endregion
}
