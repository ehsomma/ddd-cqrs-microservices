using Records.Shared.Domain.ValueObjects;

namespace Records.Persons.Domain.Persons.ValueObjects;

/// <summary>
/// Represents the FullName value object.
/// </summary>
public sealed class FullName : StringValueObject
{
    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="FullName"/> value object.
    /// </summary>
    /// <param name="value">The person fullname.</param>
    private FullName(string? value)
        : base(value, "FullName", 62, 1)
    {
    }

    /// <summary>
    /// Build a new <see cref="FullName"/> instance based on the specified <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The person fullname.</param>
    /// <returns>The value object.</returns>
    public static FullName Build(string? value)
    {
        return new FullName(value);
    }

    #endregion
}
