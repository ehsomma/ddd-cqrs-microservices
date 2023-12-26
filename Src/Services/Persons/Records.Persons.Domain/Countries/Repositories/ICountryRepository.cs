#region Usings

using Records.Persons.Domain.Countries.Models;
using Records.Persons.Domain.Countries.ValueObjects;
using Records.Persons.Domain.Shared.ValueObjects;

#endregion

namespace Records.Persons.Domain.Countries.Repositories;

/// <summary>
/// Defines the repository for <see cref="Country"/>.
/// </summary>
public interface ICountryRepository
{
    /// <summary>
    /// Inserts the specified <see cref="Country"/> into the repository.
    /// </summary>
    /// <param name="country">The Country to insert.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task InsertAsync(Country country);

    /// <summary>
    /// Update the specified <see cref="Country"/> in the repository.
    /// </summary>
    /// <param name="country">The Country to update.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task UpdateAsync(Country country);

    /// <summary>
    /// Deletes the specified <see cref="Country"/> from the repository.
    /// </summary>
    /// <param name="country">The Country to delete.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task DeleteAsync(Country country);

    /// <summary>
    /// Gets a <see cref="Country"/> by IATA code.
    /// </summary>
    /// <param name="iataCode"><inheritdoc cref="Country.IataCode" path="/summary"/></param>
    /// <returns>A <see cref="Country"/> or null.</returns>
    public Task<Country?> GetByIataCodeAsync(CountryIataCode iataCode);

    /// <summary>
    /// Checks if the country with the specified IATA code exists.
    /// </summary>
    /// <param name="iataCode"><inheritdoc cref="Country.IataCode" path="/summary"/></param>
    /// <returns>True, if it exists. Otherwize, false.</returns>
    public Task<bool> Exists(CountryIataCode iataCode);

    /// <summary>
    /// Checks if the country with the specified <paramref name="name"/> exists.
    /// </summary>
    /// <param name="name"><inheritdoc cref="Country.Name" path="/summary"/></param>
    /// <returns>True, if it exists. Otherwize, false.</returns>
    public Task<bool> Exists(CountryName name);
}
