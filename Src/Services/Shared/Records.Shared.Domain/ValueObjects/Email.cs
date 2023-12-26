namespace Records.Shared.Domain.ValueObjects;

/// <summary>
/// Represents the Email value object.
/// </summary>
public sealed class Email : StringValueObject
{
    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="Email"/> value object.
    /// </summary>
    /// <param name="value">The email address.</param>
    private Email(string? value)
        : base(value, "Email", 362, 1, "^(?!.*@.*@)[a-zA-Z0-9-.]+@+[a-zA-Z0-9].*")
    {
    }

    /// <summary>
    /// Build a new <see cref="Email"/> instance based on the specified <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The email address.</param>
    /// <returns>The value object.</returns>
    public static Email Build(string? value)
    {
        return new Email(value);
    }

    #endregion
}
