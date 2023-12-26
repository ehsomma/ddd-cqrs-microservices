using Records.Shared.Domain.ValueObjects;

namespace Records.Countries.Domain.Countries.ValueObjects;

/// <summary>
/// Represents the CountryCapital value object.
/// </summary>
public sealed class CountryCapital : StringValueObject
{
    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CountryCapital"/> value object.
    /// </summary>
    /// <param name="value">The name of the capital.</param>
    private CountryCapital(string? value)
        : base(value, "Country.Capital", 60)
    {
    }

    /// <summary>
    /// Build a new <see cref="CountryCapital"/> instance based on the specified <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The name of the capital.</param>
    /// <returns>The value object.</returns>
    public static CountryCapital Build(string? value)
    {
        CountryCapital valueObject = new (value);

        return valueObject;
    }

    #endregion
}
