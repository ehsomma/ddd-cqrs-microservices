#region Usings

using Records.Countries.Domain.Countries.Events;
using Records.Countries.Domain.Countries.ValueObjects;
using Records.Shared.Domain.Exceptions;
using Records.Shared.Domain.Models;
using Throw;

#endregion

namespace Records.Countries.Domain.Countries.Models;

/// <summary>
/// Represents a Country.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "StyleCop.CSharp.OrderingRules",
    "SA1201:Elements should appear in the correct order",
    Justification = "Prefere to have the Create() and Load() inside the constructor region.")]
public sealed class Country : AggregateRoot<string>
{
    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="Country"/> class with the specified arguments.
    /// </summary>
    /// <param name="iataCode">Country IATA code.</param>
    /// <param name="name">Country name.</param>
    /// <param name="languages">Principals languages.</param>
    /// <param name="currency">Currency code.</param>
    /// <param name="capital">Capital name.</param>
    /// <param name="area">The Area of the country (in square meters).</param>
    /// <param name="population">The population.</param>
    /// <param name="neighbors">The country neighbors.</param>
    /// <param name="flagUrl">The url of the flag image.</param>
    public Country(
        CountryIataCode iataCode,
        CountryName name,
        CountryLanguages languages,
        CountryCurrency currency,
        CountryCapital capital,
        decimal area,
        int population,
        CountryNeighbors neighbors,
        CountryFlagUrl flagUrl)
        : base(iataCode)
    {
        // NOTE: For Country entity, we use the same iataCode for the entity id base class.

        EnsureMinimalRequiredData(iataCode, name);

        IataCode = iataCode;
        Name = name;
        Languages = languages;
        Currency = currency;
        Capital = capital;
        Area = area;
        Population = population;
        Neighbors = neighbors;
        FlagUrl = flagUrl;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Country"/> class with the specified arguments.
    /// (Instantiates the class, ensure the data and registers the events).
    /// </summary>
    /// <param name="iataCode">Country IATA code.</param>
    /// <param name="name">Country name.</param>
    /// <param name="languages">Principals languages.</param>
    /// <param name="currency">Currency code.</param>
    /// <param name="capital">Capital name.</param>
    /// <param name="area">The Area of the country (in square meters).</param>
    /// <param name="population">The population.</param>
    /// <param name="neighbors">The country neighbors.</param>
    /// <param name="flagUrl">The url of the flag image.</param>
    /// <returns>The created <see cref="Country"/>.</returns>
    public static Country Create(
        CountryIataCode iataCode,
        CountryName name,
        CountryLanguages languages,
        CountryCurrency currency,
        CountryCapital capital,
        decimal area,
        int population,
        CountryNeighbors neighbors,
        CountryFlagUrl flagUrl)
    {
        Country country = new (iataCode, name, languages, currency, capital, area, population, neighbors, flagUrl);

        country.RegisterDomainEvent(new CountryCreatedEvent(country));

        return country;
    }

    /// <summary>
    /// Loads a <see cref="Country"/> with the specified arguments.
    /// (Instantiates the class, ensures the data).
    /// </summary>
    /// <param name="iataCode">Country IATA code.</param>
    /// <param name="name">Country name.</param>
    /// <param name="languages">Principals languages.</param>
    /// <param name="currency">Currency code.</param>
    /// <param name="capital">Capital name.</param>
    /// <param name="area">The Area of the country (in square meters).</param>
    /// <param name="population">The population.</param>
    /// <param name="neighbors">The country neighbors.</param>
    /// <param name="flagUrl">The url of the flag image.</param>
    /// <returns>The created <see cref="Country"/>.</returns>
    public static Country Load(
        CountryIataCode iataCode,
        CountryName name,
        CountryLanguages languages,
        CountryCurrency currency,
        CountryCapital capital,
        decimal area,
        int population,
        CountryNeighbors neighbors,
        CountryFlagUrl flagUrl)
    {
        Country country = new (iataCode, name, languages, currency, capital, area, population, neighbors, flagUrl);

        return country;
    }

    #endregion

    #region Properties

    /// <summary>Country IATA code.</summary>
    public CountryIataCode IataCode { get; init; }

    /// <summary>Country name.</summary>
    public CountryName Name { get; init; }

    /// <summary>Principals languages.</summary>
    public CountryLanguages? Languages { get; init; }

    /// <summary>Currency code.</summary>
    public CountryCurrency? Currency { get; init; }

    /// <summary>Capital name.</summary>
    public CountryCapital? Capital { get; init; }

    /// <summary>The Area of the country (in square meters).</summary>
    public decimal Area { get; init; }

    /// <summary>The population.</summary>
    public int Population { get; init; }

    /// <summary>The country neighbors.</summary>
    public CountryNeighbors? Neighbors { get; init; }

    /// <summary>The url of the flag image.</summary>
    public CountryFlagUrl? FlagUrl { get; init; }

    #endregion

    #region Public methods

    /// <summary>
    /// Delete the specified <see cref="Country"/>.
    /// </summary>
    /// <param name="country">The <see cref="Country"/> to delete.</param>
    public void Delete(Country country)
    {
        // Do some validation/ensures.
        // Update status if you need logic delete.

        RegisterDomainEvent(new CountryDeletedEvent(this));
    }

    #endregion

    #region Private methods

    /// <summary>
    /// Ensures the minimum required data are valid.
    /// </summary>
    /// <param name="iataCode">Country IATA code.</param>
    /// <param name="name">Country name.</param>
    /// <exception cref="DomainValidationException">If some argument is invalid.</exception>
    private void EnsureMinimalRequiredData(CountryIataCode iataCode, CountryName name)
    {
        try
        {
            iataCode.ThrowIfNull();
            name.ThrowIfNull();
        }
        catch (Exception ex)
        {
            throw new DomainValidationException(ex.Message, ex);
        }
    }

    #endregion
}
