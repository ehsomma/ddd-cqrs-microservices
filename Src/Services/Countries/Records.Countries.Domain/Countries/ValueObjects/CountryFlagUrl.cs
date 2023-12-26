using Records.Shared.Domain.ValueObjects;

namespace Records.Countries.Domain.Countries.ValueObjects;

/// <summary>
/// Represents the CountryFlagUrl value object.
/// </summary>
// TODO: Use System.Uri not a string.
public sealed class CountryFlagUrl : StringValueObjectNullable
{
    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CountryFlagUrl"/> value object.
    /// </summary>
    /// <param name="value">The url of the flag image.</param>
    private CountryFlagUrl(string? value)
        : base(value, "Country.Capital", 510, 0, true, "^(https:\\/\\/|HTTPS:\\/\\/)")
    {
    }

    /// <summary>
    /// Build a new <see cref="CountryFlagUrl"/> instance based on the specified <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The url of the flag image.</param>
    /// <returns>The value object.</returns>
    public static CountryFlagUrl Build(string? value)
    {
        CountryFlagUrl valueObject = new (value);

        return valueObject;
    }

    #endregion
}
