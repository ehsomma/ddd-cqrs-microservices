using Dapper.Contrib.Extensions;

namespace Records.Persons.Infra.Projection.sql.Persons.Models;

/// <summary>
/// Represents the PersonalAsset data model for table PersonalAssets (projection database).
/// </summary>
[Table("PersonalAssets")]
public class PersonalAsset
{
    #region Properties

    /// <summary>The personal asset ID.</summary>
    ////[Write(false)] // Autoincrement.
    [ExplicitKey] // Not autoincrement (autoincrement comes from _Source database).
    public int? Id { get; init; }

    /// <summary>The person ID that the personal asset belongs.</summary>
    public Guid PersonId { get; set; }

    /// <summary>The description.</summary>
    public string? Description { get; init; }

    /// <summary>The value.</summary>
    public decimal Value { get; init; }

    #endregion
}
