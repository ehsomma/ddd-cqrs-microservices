#region Usings

using Dapper.Contrib.Extensions;

#endregion

namespace Records.Persons.Infra.Projection.sql.Persons.Models;

/// <summary>
/// Represents the Person data model for table Persons (projection database).
/// </summary>
[Table("Persons")]
public class Person
{
    #region Properties

    /// <summary>The Person ID.</summary>
    ////[Key]
    [ExplicitKey] // For GUIDs (Dapper.Contrib).
    public Guid Id { get; init; }

    /// <summary>The <see cref="Records.Persons.Infra.Projection.sql.Persons.Models.Address"/> of the person.</summary>
    [Write(false)]
    public Address? Address { get; set; }

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

    /// <summary>The <see cref="PersonalAsset"/> list.</summary>
    [Write(false)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Usage",
        "CA2227:Collection properties should be read only",
        Justification = "We use it in mapper.")]
    public IList<PersonalAsset>? PersonalAssets { get; set; } = new List<PersonalAsset>();

    #endregion
}
