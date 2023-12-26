namespace Records.Shared.IntegrationEvents.Countries;

/// <summary>
/// Represents an integration event to indicate that a Country (from Countries microservice) was deleted.
/// </summary>
/// <remarks>
/// [IMPORTANT] There are multiple CountryDeletedIntegrationEvent with different namespace:
///
/// • Countries.CountryDeletedIntegrationEvent published from Countries microservice.
/// • Persons.CountryDeletedIntegrationEvent published from Persons microservice.
/// </remarks>
public sealed class CountryDeletedIntegrationEvent : IntegrationEvent
{
    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CountryDeletedIntegrationEvent"/> class.
    /// </summary>
    /// <param name="iataCode">Country IATA code.</param>
    public CountryDeletedIntegrationEvent(string iataCode)
    {
        IataCode = iataCode;
    }

    #endregion

    #region Properties

    /// <summary>Gets prueba uno.</summary>
    public string IataCode { get; }

    #endregion
}
