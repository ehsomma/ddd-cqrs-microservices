using Records.Persons.Domain.Countries.Models;
using Records.Persons.Domain.Countries.ValueObjects;
using Records.Persons.Domain.Persons.Models;
using Records.Persons.Domain.Shared.ValueObjects;
using Records.Shared.Domain.Exceptions;
using Records.Shared.Domain.Services;

namespace Records.Persons.Domain.Countries.Services;

/// <summary>
/// Defines the country service.
/// </summary>
public interface ICountryService : IDomainService
{
    /// <summary>
    /// Gets the <see cref="Country"/> corresponding to the specified <paramref name="iataCode"/>.
    /// </summary>
    /// <param name="iataCode">The country IATA code to search for.</param>
    /// <returns>A <see cref="Country"/> or null.</returns>
    /// <exception cref="DomainException">When the country is not found.</exception>
    Task<Country?> GetByIataCodeAsync(CountryIataCode iataCode);

    /// <summary>
    /// Checks if the country with the specified <paramref name="iataCode"/> exists in the repository.
    /// </summary>
    /// <param name="iataCode">The IATA code of the country to check.</param>
    /// <returns>True, if the country exists. Otherwise, false.</returns>
    Task<bool> Exists(CountryIataCode iataCode);

    /// <summary>
    /// Checks if the country with the specified <paramref name="countryName"/> exists in the repository.
    /// </summary>
    /// <param name="countryName">The name of the country to check.</param>
    /// <returns>True, if the country exists. Otherwise, false.</returns>
    Task<bool> Exists(CountryName countryName);

    /// <summary>
    /// Ensures that a country with the specified <paramref name="countryName"/> exists in the
    /// repository. Throws a DomainException if the country does not exist.
    /// </summary>
    /// <param name="countryName">The name of the country to check.</param>
    /// <exception cref="DomainException">When the country is not found.</exception>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task EnsureExists(CountryName countryName);

    /// <summary>
    /// Ensures that a country of the specified <paramref name="address"/> exists in the repository.
    /// Throws a DomainException if the country does not exist.
    /// </summary>
    /// <param name="address">The address that contains the country to check.</param>
    /// <exception cref="DomainException">When the country is not found.</exception>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task EnsureExists(Address address);
}
