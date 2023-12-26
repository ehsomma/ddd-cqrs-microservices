using Records.Shared.Domain.ValueObjects;

namespace Records.Persons.Domain.Persons.ValueObjects;

/// <summary>
/// Represents the Email value object.
/// </summary>
public sealed class PersonalAssetDescription : StringValueObject
{
    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="PersonalAssetDescription"/> value object.
    /// </summary>
    /// <param name="value">The description.</param>
    private PersonalAssetDescription(string? value)
        : base(value, "PersonalAssetDescription", 255, 1)
    {
    }

    /// <summary>
    /// Build a new <see cref="PersonalAssetDescription"/> instance based on the specified <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The description.</param>
    /// <returns>The value object.</returns>
    public static PersonalAssetDescription Build(string? value)
    {
        return new PersonalAssetDescription(value);
    }

    #endregion
}
