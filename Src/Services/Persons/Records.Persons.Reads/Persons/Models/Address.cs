namespace Records.Persons.Reads.Persons.Models;

/// <summary>
/// Represents an address (from projection database).
/// </summary>
public class Address
{
    #region Properties

    /// <summary>The id of the address.</summary>
    public int Id { get; init; } // Autoincrement (from source).

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

    /// <summary>Represents the <see cref="Records.Persons.Reads.Persons.Models.LatLng"/> (geographical point on Earth).</summary>
    public LatLng? LatLng { get; set; }

    #endregion
}
