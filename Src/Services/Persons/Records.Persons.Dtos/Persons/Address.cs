using Records.Shared.Infra.Swagger.Schemas;

namespace Records.Persons.Dtos.Persons;

/// <summary>
/// Represents an address.
/// </summary>
public class Address
{
    #region Properties

    /// <summary>The id of the address.</summary>
    public int Id { get; init; }

    /// <summary>The street line 1.</summary>
    [SwaggerSchemaExample("4441 Collins Avenue")]
    public string? StreetLine1 { get; init; }

    /// <summary>The street line 2.</summary>
    [SwaggerSchemaExample(null!)]
    public string? StreetLine2 { get; init; }

    /// <summary>The name of the city.</summary>
    [SwaggerSchemaExample("Miami Beach")]
    public string? City { get; init; }

    /// <summary>The name of the state.</summary>
    [SwaggerSchemaExample("FL")]
    public string? State { get; init; }

    /// <summary>The name of the country.</summary>
    [SwaggerSchemaExample("United States")]
    public string? Country { get; init; }

    /// <summary>Represents the <see cref="Records.Persons.Dtos.Persons.LatLng"/> (geographical point on Earth).</summary>
    public LatLng? LatLng { get; init; }

    #endregion
}
