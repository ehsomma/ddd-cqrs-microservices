using Records.Shared.Domain.ValueObjects;

namespace Records.Persons.Domain.Persons.ValueObjects;

/// <summary>
/// Represents the StreetLine2 value object.
/// </summary>
/// <remarks>
/// Value could be:
/// • null
/// • "..." length > 1
///
/// Value could not be:
/// • "".
/// </remarks>
public sealed class StreetLine2 : StringValueObjectNullable
{
    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="StreetLine2"/> value object.
    /// </summary>
    /// <param name="value">The street line.</param>
    private StreetLine2(string? value)
        : base(value, "StreetLine2", 60, 1) // nullable, but is not, grater than 1.
    {
    }

    /// <summary>
    /// Build a new <see cref="StreetLine2"/> instance based on the specified <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The street line.</param>
    /// <returns>The value object.</returns>
    public static StreetLine2 Build(string? value)
    {
        return new StreetLine2(value);
    }

    #endregion
}
