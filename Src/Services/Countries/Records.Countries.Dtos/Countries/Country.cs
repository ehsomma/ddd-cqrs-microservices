using Records.Shared.Infra.Swagger.Schemas;

namespace Records.Countries.Dtos.Countries;

/// <summary>
/// Represents a Country.
/// </summary>
public class Country
{
    /// <summary>Country IATA code.</summary>
    [SwaggerSchemaExample("US")]
    public string? IataCode { get; init; }

    /// <summary>Country name.</summary>
    [SwaggerSchemaExample("United States")]
    public string? Name { get; init; }

    /// <summary>Country principal languages.</summary>
    [SwaggerSchemaExample("en")]
    public string? Languages { get; init; }

    /// <summary>Country currency.</summary>
    [SwaggerSchemaExample("USD")]
    public string? Currency { get; init; }

    /// <summary>Country capital.</summary>
    [SwaggerSchemaExample("Washington, D.C.")]
    public string? Capital { get; init; }

    /// <summary>Area (in km2).</summary>
    [SwaggerSchemaExample("9833520")]
    public decimal Area { get; init; }

    /// <summary>Country population.</summary>
    [SwaggerSchemaExample("333287557")]
    public int Population { get; init; }

    /// <summary>Country neighbors.</summary>
    [SwaggerSchemaExample("Canada, Mexico")]
    public string? Neighbors { get; init; }

    /// <summary>Country flag URL.</summary>
    [SwaggerSchemaExample("https://en.wikipedia.org/US_flag.svg")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Design",
        "CA1056:URI-like properties should not be strings",
        Justification = "TODO: Use System.Uri not string.")]
    public string? FlagUrl { get; init; }
}
