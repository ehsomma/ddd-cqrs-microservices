namespace Records.Persons.Reads.Persons.Models;

/// <summary>
/// Represents the <see cref="LatLng"/> (geographical point on Earth) (from projection database).
/// </summary>
public class LatLng
{
    #region Properties

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
