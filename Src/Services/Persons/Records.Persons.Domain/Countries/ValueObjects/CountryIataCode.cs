using Records.Shared.Domain.ValueObjects;

namespace Records.Persons.Domain.Countries.ValueObjects;

/// <summary>
/// Represents the CountryIataCode value object.
/// </summary>
public sealed class CountryIataCode : StringValueObject
{
    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CountryIataCode"/> value object.
    /// </summary>
    /// <param name="value">The country IATA code.</param>
    private CountryIataCode(string? value)
        : base(value, "IataCode", 2, 2)
    {
    }

    /// <summary>
    /// Build a new <see cref="CountryIataCode"/> instance based on the specified <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The country IATA code.</param>
    /// <returns>The value object.</returns>
    public static CountryIataCode Build(string? value)
    {
        CountryIataCode valueObject = new (value);

        return valueObject;
    }

    #endregion
}
