using Records.Shared.Infra.Swagger.Schemas;

namespace Records.Persons.Dtos.Countries;

/// <summary>
/// Represents a Country.
/// </summary>
public class Country
{
    #region Properties

    /// <summary>Country IATA code.</summary>
    [SwaggerSchemaExample("US")]
    public string? IataCode { get; init; }

    /// <summary>Country name.</summary>
    [SwaggerSchemaExample("United States")]
    public string? Name { get; init; }

    #endregion
}
