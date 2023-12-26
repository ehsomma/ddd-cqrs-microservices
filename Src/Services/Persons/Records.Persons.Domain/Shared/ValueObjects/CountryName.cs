using Records.Shared.Domain.ValueObjects;

namespace Records.Persons.Domain.Shared.ValueObjects;

/// <summary>
/// Represents the CountryName value object.
/// </summary>
public sealed class CountryName : StringValueObject
{
    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CountryName"/> value object.
    /// </summary>
    /// <param name="value">The country name.</param>
    private CountryName(string? value)
        : base(value, "Country.Name", 160, 0)
    {
    }

    /// <summary>
    /// Build a new <see cref="CountryName"/> instance based on the specified <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The country name.</param>
    /// <returns>The value object.</returns>
    public static CountryName Build(string? value)
    {
        CountryName valueObject = new (value);

        return valueObject;
    }

    #endregion
}
