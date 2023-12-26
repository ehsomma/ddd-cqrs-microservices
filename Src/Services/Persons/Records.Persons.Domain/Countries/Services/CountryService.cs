#region Usings

using Records.Persons.Domain.Countries.Errors;
using Records.Persons.Domain.Countries.Models;
using Records.Persons.Domain.Countries.Repositories;
using Records.Persons.Domain.Countries.ValueObjects;
using Records.Persons.Domain.Persons.Models;
using Records.Persons.Domain.Shared.ValueObjects;
using Records.Shared.Domain.Exceptions;

#endregion

namespace Records.Persons.Domain.Countries.Services;

// Services policies:
// • Contains domain operations with access to the repository.
// • Contains non basic operations that use other aggregates.
// • Receives the repository by constructor.
// • Can not use other repos.

/// <summary>
/// Represents the country service.
/// </summary>
public sealed class CountryService : ICountryService
{
    #region Declarations

    /// <summary>Represents the <see cref="Country"/> persistence operations.</summary>
    private readonly ICountryRepository _repository;

    #endregion

    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CountryService"/> class.
    /// </summary>
    /// <param name="repository">Represents the <see cref="Country"/> persistence operations.</param>
    /// <exception cref="ArgumentNullException">When some argument for the constructor parameters is null.</exception>
    public CountryService(ICountryRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Gets the <see cref="Country"/> corresponding to the specified <paramref name="iataCode"/>.
    /// </summary>
    /// <param name="iataCode">The country IATA code to search for.</param>
    /// <returns>A <see cref="Country"/> or null.</returns>
    /// <exception cref="DomainException">When the country is not found.</exception>
    public async Task<Country?> GetByIataCodeAsync(CountryIataCode iataCode)
    {
        Country? country = await _repository.GetByIataCodeAsync(iataCode);

        if (country is null)
        {
            throw new DomainException(DomainErrors.Country.NotFound);
        }

        return country;
    }

    /// <summary>
    /// Checks if the country with the specified <paramref name="iataCode"/> exists in the repository.
    /// </summary>
    /// <param name="iataCode">The IATA code of the country to check.</param>
    /// <returns>True, if the country exists. Otherwise, false.</returns>
    public async Task<bool> Exists(CountryIataCode iataCode)
    {
        return await _repository.Exists(iataCode);
    }

    /// <summary>
    /// Checks if the country with the specified <paramref name="countryName"/> exists in the repository.
    /// </summary>
    /// <param name="countryName">The name of the country to check.</param>
    /// <returns>True, if the country exists. Otherwise, false.</returns>
    public async Task<bool> Exists(CountryName countryName)
    {
        return await _repository.Exists(countryName);
    }

    /// <summary>
    /// Ensures that a country with the specified <paramref name="countryName"/> exists in the
    /// repository. Throws a DomainException if the country does not exist.
    /// </summary>
    /// <param name="countryName">The name of the country to check.</param>
    /// <exception cref="DomainException">When the country is not found.</exception>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task EnsureExists(CountryName countryName)
    {
        bool exists = await Exists(countryName);

        if (!exists)
        {
            throw new DomainException(DomainErrors.Country.NotFound);
        }
    }

    /// <summary>
    /// Ensures that a country of the specified <paramref name="address"/> exists in the repository.
    /// Throws a DomainException if the country does not exist.
    /// </summary>
    /// <param name="address">The address that contains the country to check.</param>
    /// <exception cref="DomainException">When the country is not found.</exception>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task EnsureExists(Address address)
    {
        if (address?.Country != null)
        {
            await EnsureExists(address.Country);
        }
    }

    #endregion
}
