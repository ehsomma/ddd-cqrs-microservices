#region Usings

using Records.Persons.Domain.Countries.Events;
using Records.Persons.Domain.Countries.ValueObjects;
using Records.Persons.Domain.Shared.ValueObjects;
using Records.Shared.Domain.Exceptions;
using Records.Shared.Domain.Models;
using Throw;

#endregion

namespace Records.Persons.Domain.Countries.Models;

/// <summary>
/// Represents a Country.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "StyleCop.CSharp.OrderingRules",
    "SA1201:Elements should appear in the correct order",
    Justification = "Prefere to have the Create() and Load() inside the constructor region.")]
public sealed class Country : AggregateRoot<string>
{
    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="Country"/> class.
    /// </summary>
    /// <param name="iataCode">Country IATA code.</param>
    /// <param name="name">Country name.</param>
    private Country(
        CountryIataCode iataCode,
        CountryName name)
            : base(iataCode)
    {
        // NOTE: For Country entity, we use the same iataCode for the entity id base class.

        EnsureMinimalRequiredData(iataCode, name);

        IataCode = iataCode;
        Name = name;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Country"/> class with the specified arguments.
    /// (Instantiates the class, ensure the data and registers the events).
    /// </summary>
    /// <param name="iataCode">Country IATA code.</param>
    /// <param name="name">Country name.</param>
    /// <returns>The created <see cref="Country"/>.</returns>
    public static Country Create(
        CountryIataCode iataCode,
        CountryName name)
    {
        Country country = new (iataCode, name);

        country.RegisterDomainEvent(new CountryCreatedEvent(country));

        return country;
    }

    /// <summary>
    /// Loads a new instance of the <see cref="Country"/> class with the specified arguments.
    /// (Instantiates the class, ensure the data).
    /// </summary>
    /// <param name="iataCode">Country IATA code.</param>
    /// <param name="name">Country name.</param>
    /// <returns>The loaded <see cref="Country"/>.</returns>
    public static Country Load(
        CountryIataCode iataCode,
        CountryName name)
    {
        Country country = new (iataCode, name);

        return country;
    }

    #endregion

    #region Properties

    /// <summary>Country IATA code.</summary>
    public CountryIataCode IataCode { get; private set; }

    /// <summary>Country name.</summary>
    public CountryName Name { get; private set; }

    #endregion

    #region Public methods

    /// <summary>
    /// Delete the specified <see cref="Country"/>.
    /// </summary>
    /// <param name="country">The <see cref="Country"/> to delete.</param>
    public void Delete(Country country)
    {
        // Do some validation/ensures.
        // Update status if you need delete logic.

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
    private void EnsureMinimalRequiredData(CountryIataCode iataCode, string name)
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
