using Records.Shared.Domain.ValueObjects;

namespace Records.Persons.Domain.Persons.ValueObjects;

/// <summary>
/// Represents the City value object.
/// </summary>
public sealed class City : StringValueObject
{
    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="City"/> value object.
    /// </summary>
    /// <param name="value">The email address.</param>
    private City(string? value)
        : base(value, "City", 50, 1)
    {
    }

    /// <summary>
    /// Build a new <see cref="City"/> instance based on the specified <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The city name.</param>
    /// <returns>The value object.</returns>
    public static City Build(string? value)
    {
        return new City(value);
    }

    #endregion
}
