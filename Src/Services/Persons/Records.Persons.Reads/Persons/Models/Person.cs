namespace Records.Persons.Reads.Persons.Models;

/// <summary>
/// Represents a Person (from projection database).
/// </summary>
public class Person
{
    #region Properties

    /// <summary>The Person ID.</summary>
    public Guid Id { get; init; }

    /// <summary>The full name.</summary>
    public string FullName { get; init; } = string.Empty;

    /// <summary>The email.</summary>
    public string? Email { get; init; }

    /// <summary>The phone number.</summary>
    public string? Phone { get; init; }

    /// <summary>The gender.</summary>
    public string? Gender { get; init; }

    /// <summary>The birthdate.</summary>
    public DateTime? Birthdate { get; init; }

    /// <summary>The <see cref="Records.Persons.Reads.Persons.Models.Address"/> of the person.</summary>
    public Address? Address { get; set; }

    /// <summary>The <see cref="PersonalAsset"/> list.</summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Usage",
        "CA2227:Collection properties should be read only",
        Justification = "We use it in mapper.")]
    public IList<PersonalAsset>? PersonalAssets { get; set; } = new List<PersonalAsset>();

    #endregion
}
