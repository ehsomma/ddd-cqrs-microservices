using Records.Shared.Infra.Swagger.Schemas;

namespace Records.Persons.Dtos.Persons;

/// <summary>
/// Represents the <see cref="LatLng"/> (geographical point on Earth).
/// </summary>
public class LatLng
{
    #region Properties

    /// <summary>
    /// Latitude of the geographical point (angular distance from a point on the Earth's surface to
    /// the parallel of the equator). In WGS-84 format (e.g. 25.796549. "-90" to "90").
    /// </summary>
    [SwaggerSchemaExample("25.817887")]
    public decimal? Lat { get; init; }

    /// <summary>
    /// Geographic point longitude (angular distance from a point on the Earth's surface to the
    /// Greenwich meridian). In WGS-84 format (e.g. -80.275613. "-180", "180").
    /// </summary>
    [SwaggerSchemaExample("-80.122785")]
    public decimal? Lng { get; init; }

    #endregion
}
