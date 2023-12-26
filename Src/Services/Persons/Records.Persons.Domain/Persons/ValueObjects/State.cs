using Records.Shared.Domain.ValueObjects;

namespace Records.Persons.Domain.Persons.ValueObjects;

/// <summary>
/// Represents the State value object.
/// </summary>
public sealed class State : StringValueObject
{
    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="State"/> value object.
    /// </summary>
    /// <param name="value">The state name.</param>
    private State(string? value)
        : base(value, "State", 50, 1)
    {
    }

    /// <summary>
    /// Build a new <see cref="State"/> instance based on the specified <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The state name.</param>
    /// <returns>The value object.</returns>
    public static State Build(string? value)
    {
        return new State(value);
    }

    #endregion
}
