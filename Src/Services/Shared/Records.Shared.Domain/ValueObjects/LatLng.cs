#region Usings

using Records.Shared.Domain.Exceptions;
using Records.Shared.Domain.Models;
using System.Globalization;
using Throw;

#endregion

namespace Records.Shared.Domain.ValueObjects;

/// <summary>
/// Represents the <see cref="LatLng"/> (geographical point on Earth) value object.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "StyleCop.CSharp.OrderingRules",
    "SA1201:Elements should appear in the correct order",
    Justification = "Prefere to have the Build() inside the constructor region.")]
public sealed class LatLng : ValueObject
{
    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="LatLng"/> value object.
    /// </summary>
    /// <param name="lat">Latitude of the geographical point (angular distance from a point on the Earth's surface to the parallel of the equator). In WGS-84 format (e.g. 25.796549. "-90" to "90").</param>
    /// <param name="lng">Geographic point longitude (angular distance from a point on the Earth's surface to the Greenwich meridian). In WGS-84 format (e.g. -80.275613. "-180", "180").</param>param>
    private LatLng(decimal? lat, decimal? lng)
    {
        EnsureCoordinatesLimits(lat, lng);

        Lat = (decimal)lat!;
        Lng = (decimal)lng!;
    }

    /// <summary>
    /// Build a new <see cref="LatLng"/> instance based on the specified <paramref name="lat"/> and <paramref name="lng"/>.
    /// </summary>
    /// <param name="lat">Latitude of the geographical point (angular distance from a point on the Earth's surface to the parallel of the equator). In WGS-84 format (e.g. 25.796549. "-90" to "90").</param>
    /// <param name="lng">Geographic point longitude (angular distance from a point on the Earth's surface to the Greenwich meridian). In WGS-84 format (e.g. -80.275613. "-180", "180").</param>
    /// <returns>The value object.</returns>
    public static LatLng Build(decimal? lat, decimal? lng)
    {
        LatLng latLng = new (lat, lng);

        return latLng;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Latitude of the geographical point (angular distance from a point on the Earth's surface to
    /// the parallel of the equator). In WGS-84 format (e.g. 25.796549. "-90" to "90").
    /// </summary>
    public decimal Lat { get; private set; }

    /// <summary>
    /// Geographic point longitude (angular distance from a point on the Earth's surface to the
    /// Greenwich meridian). In WGS-84 format (e.g. -80.275613. "-180", "180").
    /// </summary>
    public decimal Lng { get; private set; }

    #endregion

    #region Methods

    /// <inheritdoc />
    public override string ToString()
    {
        string ret = $"{Lat.ToString(CultureInfo.InvariantCulture)}, {Lng.ToString(CultureInfo.InvariantCulture)}";
        return ret;
    }

    /// <inheritdoc />
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Lat;
        yield return Lng;
    }

    /// <summary>
    /// Ensures that the arguments to create the <see cref="LatLng"/> value object are valid.
    /// </summary>
    /// <param name="lat">Latitude of the geographical point (angular distance from a point on the Earth's surface to the parallel of the equator). In WGS-84 format (e.g. 25.796549. "-90" to "90").</param>
    /// <param name="lng">Geographic point longitude (angular distance from a point on the Earth's surface to the Greenwich meridian). In WGS-84 format (e.g. -80.275613. "-180", "180").</param>
    /// <exception cref="DomainValidationException">If some argument is invalid.</exception>
    private void EnsureCoordinatesLimits(decimal? lat, decimal? lng)
    {
        try
        {
            lat.ThrowIfNull().IfLessThan(-90).IfGreaterThan(90); // -90 <=> 90.
            lng.ThrowIfNull().IfLessThan(-180).IfGreaterThan(180); // -180 <=> 180.
        }
        catch (Exception ex)
        {
            throw new DomainValidationException(ex.Message, ex);
        }
    }

    #endregion
}
