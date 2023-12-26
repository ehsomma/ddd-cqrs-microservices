#region Usings

using Records.Persons.Domain.Persons.ValueObjects;
using Records.Persons.Domain.Shared.ValueObjects;
using Records.Shared.Domain.Exceptions;
using Records.Shared.Domain.Models;
using Records.Shared.Domain.ValueObjects;
using Throw;

#endregion

namespace Records.Persons.Domain.Persons.Models;

/// <summary>
/// Represents an Address.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "StyleCop.CSharp.OrderingRules",
    "SA1201:Elements should appear in the correct order",
    Justification = "Prefere to have the Create() and Load() inside the constructor region.")]
public sealed class Address : Entity<int>
{
    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="Address"/> class with the specified arguments.
    /// </summary>
    /// <param name="id">The address ID.</param>
    /// <param name="streetLine1">The street line 1.</param>
    /// <param name="streetLine2">The street line 2.</param>
    /// <param name="city">The name of the city.</param>
    /// <param name="state">The name of the state.</param>
    /// <param name="country">The name of the country.</param>
    /// <param name="latLng">The <see cref="Records.Shared.Domain.ValueObjects.LatLng"/> (geographical point on Earth).</param>
    private Address(int id, StreetLine? streetLine1, StreetLine2? streetLine2, City? city, State? state, CountryName? country, LatLng? latLng)
        : base(id)
    {
        EnsureAddressMinimalRequiredData(streetLine1);

        Id = id;
        StreetLine1 = streetLine1!; // Validated.
        StreetLine2 = streetLine2;
        City = city;
        State = state;
        Country = country;
        LatLng = latLng;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Address"/> class with the specified arguments.
    /// (Instantiates the class, ensure the data and registers the events).
    /// </summary>
    /// <param name="streetLine1">The street line 1.</param>
    /// <param name="streetLine2">The street line 2.</param>
    /// <param name="city">The name of the city.</param>
    /// <param name="state">The name of the state.</param>
    /// <param name="country">The name of the country.</param>
    /// <param name="latLng">The <see cref="Records.Shared.Domain.ValueObjects.LatLng"/> (geographical point on Earth).</param>
    /// <returns>The created <see cref="Address"/>.</returns>
    public static Address Create(StreetLine? streetLine1, StreetLine2? streetLine2, City? city, State? state, CountryName? country, LatLng? latLng)
    {
        Address address = new (0, streetLine1, streetLine2, city, state, country, latLng);

        // Registers domain events.
        ////address.RegisterDomainEvent(new AddressCreatedEvent(address));

        return address;
    }

    /// <summary>
    /// Loads a new instance of the <see cref="Address"/> class with the specified arguments.
    /// (Instantiates the class, ensure the data).
    /// </summary>
    /// <param name="id">The address ID.</param>
    /// <param name="streetLine1">The street line 1.</param>
    /// <param name="streetLine2">The street line 2.</param>
    /// <param name="city">The name of the city.</param>
    /// <param name="state">The name of the state.</param>
    /// <param name="country">The name of the country.</param>
    /// <param name="latLng">The <see cref="Records.Shared.Domain.ValueObjects.LatLng"/> (geographical point on Earth).</param>
    /// <returns>The loaded <see cref="Address"/>.</returns>
    public static Address Load(int id, StreetLine? streetLine1, StreetLine2? streetLine2, City? city, State? state, CountryName? country, LatLng? latLng)
    {
        Address address = new (id, streetLine1, streetLine2, city, state, country, latLng);

        return address;
    }

    #endregion

    #region Properties

    ////public int Id { get; private set; } // Defined in the base class.

    /// <summary>The street line 1.</summary>
    public StreetLine StreetLine1 { get; private set; }

    /// <summary>The street line 2.</summary>
    public StreetLine2? StreetLine2 { get; }

    /// <summary>The name of the city.</summary>
    public City? City { get; }

    /// <summary>The name of the state.</summary>
    public State? State { get; }

    /// <summary>The name of the country.</summary>
    public CountryName? Country { get; }

    /// <summary>The <see cref="Records.Shared.Domain.ValueObjects.LatLng"/> (geographical point on Earth).</summary>
    public LatLng? LatLng { get; }

    #endregion

    #region Methods

    /// <summary>
    /// Ensures the minimum required data are valid.
    /// </summary>
    /// <param name="streetLine1">The street line 1.</param>
    /// <exception cref="DomainValidationException">If some argument is invalid.</exception>
    private void EnsureAddressMinimalRequiredData(StreetLine? streetLine1)
    {
        try
        {
            streetLine1.ThrowIfNull();
        }
        catch (Exception ex)
        {
            throw new DomainValidationException(ex.Message, ex);
        }
    }

    #endregion
}
