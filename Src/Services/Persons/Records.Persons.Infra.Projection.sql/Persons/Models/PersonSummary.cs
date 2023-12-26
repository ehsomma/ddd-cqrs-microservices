using Dapper.Contrib.Extensions;

namespace Records.Persons.Infra.Projection.sql.Persons.Models;

/// <summary>
/// Represents the PersonSummary data model for table PersonsSummary (projection database).
/// </summary>
[Table("PersonsSummary")]
public class PersonSummary
{
    #region Properties

    /// <summary>The Person ID.</summary>
    [ExplicitKey] // For GUIDs (Dapper.Contrib).
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

    /// <summary>The name of the city.</summary>
    public string? City { get; init; }

    /// <summary>The name of the state.</summary>
    public string? State { get; init; }

    /// <summary>The name of the country.</summary>
    public string? Country { get; init; }

    /// <summary>
    /// Latitude of the geographical point (angular distance from a point on the Earth's surface to
    /// the parallel of the equator). In WGS-84 format (e.g. 25.796549. "-90" to "90").
    /// </summary>
    public decimal? Lat { get; init; }

    /// <summary>
    /// Geographic point longitude (angular distance from a point on the Earth's surface to the
    /// Greenwich meridian). In WGS-84 format (e.g. -80.275613. "-180", "180").
    /// </summary>
    public decimal? Lng { get; init; }

    /// <summary>Personal Assets count.</summary>
    public decimal PersonalAssetsCount { get; init; } = 0;

    /// <summary>Personal Assets balance.</summary>
    public decimal PersonalAssetsBalance { get; init; } = 0;

    #endregion
}
