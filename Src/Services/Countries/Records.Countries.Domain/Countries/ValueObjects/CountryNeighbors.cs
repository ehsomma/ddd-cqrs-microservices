using Records.Shared.Domain.ValueObjects;

namespace Records.Countries.Domain.Countries.ValueObjects;

/// <summary>
/// Represents the CountryNeighbors value object.
/// </summary>
public sealed class CountryNeighbors : StringValueObject
{
    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CountryNeighbors"/> value object.
    /// </summary>
    /// <param name="value">The name of the country neighbors.</param>
    private CountryNeighbors(string? value)
        : base(value, "Country.Neighbors", 255)
    {
    }

    /// <summary>
    /// Build a new <see cref="CountryNeighbors"/> instance based on the specified <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The name of the country neighbors.</param>
    /// <returns>The value object.</returns>
    public static CountryNeighbors Build(string? value)
    {
        CountryNeighbors valueObject = new (value);

        return valueObject;
    }

    #endregion
}
