namespace Records.Persons.Reads.Persons.Models;

/// <summary>
/// Represents a personal asset (from projection database).
/// </summary>
public class PersonalAsset
{
    #region Properties

    /// <summary>Personal asset ID.</summary>
    public int Id { get; init; }

    /// <summary>The description.</summary>
    public string? Description { get; init; }

    /// <summary>The value.</summary>
    public decimal Value { get; init; }

    #endregion
}
