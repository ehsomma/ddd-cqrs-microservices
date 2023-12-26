namespace Records.Shared.Infra.Swagger.Schemas;

/// <summary>
/// Represents a custom attribute to provide examples on Swagger UI from the class used to requests
/// and responses.
/// </summary>
[AttributeUsage(
    AttributeTargets.Class |
    AttributeTargets.Struct |
    AttributeTargets.Parameter |
    AttributeTargets.Property |
    AttributeTargets.Enum,
    AllowMultiple = false)]
public sealed class SwaggerSchemaExampleAttribute : Attribute
{
    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="SwaggerSchemaExampleAttribute"/> class.
    /// </summary>
    /// <param name="example">The example value to show in teh Swagger UI.</param>
    public SwaggerSchemaExampleAttribute(string example)
    {
        Example = example;
    }

    #endregion

    #region Properties

    /// <summary>The example value to show in teh Swagger UI.</summary>
    public string Example { get; internal set; }

    #endregion
}
