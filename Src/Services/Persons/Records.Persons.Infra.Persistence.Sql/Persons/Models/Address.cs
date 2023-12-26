using Dapper.Contrib.Extensions;

namespace Records.Persons.Infra.Persistence.Sql.Persons.Models;

/// <summary>
/// Represents the Address data model for table Addresses.
/// </summary>
[Table("Addresses")]
public class Address
{
    #region Properties

    /// <summary>The address ID.</summary>
    ////[Write(false)] // Autoincrement. // Works on insert commands, but produce error on update: "Entity must have at least one [Key] or [ExplicitKey] property".
    ////[ExplicitKey] // Doesn't works on insert commands: "Cannot insert explicit value for identity column in table 'Addresses' when IDENTITY_INSERT is set to OFF".
    [Key] // Works on insert and update commands too.
    public int Id { get; init; }

    /// <summary>The person ID that the address belongs.</summary>
    public Guid PersonId { get; init; }

    /// <summary>The street line 1.</summary>
    public string? StreetLine1 { get; init; }

    /// <summary>The street line 2.</summary>
    public string? StreetLine2 { get; init; }

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

    #endregion
}
