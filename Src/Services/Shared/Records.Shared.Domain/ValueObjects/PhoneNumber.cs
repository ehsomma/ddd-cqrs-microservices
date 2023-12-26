namespace Records.Shared.Domain.ValueObjects;

/// <summary>
/// Represents the PhoneNumber value object.
/// </summary>
public sealed class PhoneNumber : StringValueObjectNullable
{
    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="PhoneNumber"/> value object.
    /// </summary>
    /// <param name="value">The phone number.</param>
    private PhoneNumber(string? value)
        : base(value, "PhoneNumber", 60, 0)
    {
    }

    /// <summary>
    /// Build a new <see cref="PhoneNumber"/> instance based on the specified <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The phone number.</param>
    /// <returns>The value object.</returns>
    public static PhoneNumber Build(string? value)
    {
        return new PhoneNumber(value);
    }

    #endregion
}
