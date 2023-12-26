using Records.Shared.Infra.Swagger.Schemas;

namespace Records.Persons.Dtos.Persons;

/// <summary>
/// Represents a personal asset.
/// </summary>
public class PersonalAsset
{
    #region Properties

    /// <summary>The PersonalAsset ID.</summary>
    [SwaggerSchemaExample("1")]
    public int Id { get; init; } // Autoincrement.

    /// <summary>The description.</summary>
    [SwaggerSchemaExample("Oculus Quest 2")]
    public string? Description { get; init; }

    /// <summary>The monetary value.</summary>
    [SwaggerSchemaExample("400")]
    public decimal Value { get; init; }

    #endregion
}
