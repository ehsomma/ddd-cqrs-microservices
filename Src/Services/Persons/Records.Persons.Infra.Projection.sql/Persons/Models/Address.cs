using Dapper.Contrib.Extensions;

namespace Records.Persons.Infra.Projection.sql.Persons.Models;

/// <summary>
/// Represents the Address data model for table Addresses (projection database).
/// </summary>
[Table("Addresses")]
public class Address
{
    /// <summary>The address ID.</summary>
    [ExplicitKey] // Not autoincrement (autoincrement id comes from _Source database).
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
}
