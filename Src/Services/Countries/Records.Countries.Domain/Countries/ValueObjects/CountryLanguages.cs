using Records.Shared.Domain.ValueObjects;

namespace Records.Countries.Domain.Countries.ValueObjects;

/// <summary>
/// Represents the CountryLanguages value object.
/// </summary>
public sealed class CountryLanguages : StringValueObject
{
    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CountryLanguages"/> value object.
    /// </summary>
    /// <param name="value">The principal languages.</param>
    private CountryLanguages(string? value)
        : base(value, "Country.Languages", 60)
    {
    }

    /// <summary>
    /// Build a new <see cref="CountryLanguages"/> instance based on the specified <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The principal languages.</param>
    /// <returns>The value object.</returns>
    public static CountryLanguages Build(string? value)
    {
        CountryLanguages valueObject = new (value);

        return valueObject;
    }

    #endregion
}
