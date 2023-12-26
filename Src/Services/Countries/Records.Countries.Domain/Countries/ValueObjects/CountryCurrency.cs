using Records.Shared.Domain.ValueObjects;

namespace Records.Countries.Domain.Countries.ValueObjects;

/// <summary>
/// Represents the CountryCurrency value object.
/// </summary>
public sealed class CountryCurrency : StringValueObject
{
    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CountryCurrency"/> value object.
    /// </summary>
    /// <param name="value">The currencey code.</param>
    private CountryCurrency(string? value)
        : base(value, "Country.Currency", 3, 3)
    {
    }

    /// <summary>
    /// Build a new <see cref="CountryCurrency"/> instance based on the specified <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The currencey code.</param>
    /// <returns>The value object.</returns>
    public static CountryCurrency Build(string? value)
    {
        CountryCurrency valueObject = new (value);

        return valueObject;
    }

    #endregion
}
