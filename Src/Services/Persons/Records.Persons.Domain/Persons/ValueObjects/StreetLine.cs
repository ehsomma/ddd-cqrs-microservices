using Records.Shared.Domain.ValueObjects;

namespace Records.Persons.Domain.Persons.ValueObjects;

/// <summary>
/// Represents the StreetLine value object.
/// </summary>
public sealed class StreetLine : StringValueObject
{
    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="StreetLine"/> value object.
    /// </summary>
    /// <param name="value">The street line.</param>
    private StreetLine(string? value)
        : base(value, "StreetLine", 60, 1)
    {
    }

    /// <summary>
    /// Build a new <see cref="StreetLine"/> instance based on the specified <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The street line.</param>
    /// <returns>The value object.</returns>
    public static StreetLine Build(string? value)
    {
        return new StreetLine(value);
    }

    #endregion
}
