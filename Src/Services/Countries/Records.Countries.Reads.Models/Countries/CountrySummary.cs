namespace Records.Countries.Reads.Models.Countries;

/// <summary>
/// Represents a CountrySummary.
/// </summary>
public class CountrySummary
{
    #region Properties

    /// <summary>Country IATA code.</summary>
    public string? IataCode { get; init; }

    /// <summary>Country name.</summary>
    public string? Name { get; init; }

    #endregion
}
