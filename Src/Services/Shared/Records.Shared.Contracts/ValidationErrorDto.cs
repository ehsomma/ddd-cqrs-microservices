using Records.Shared.Infra.Swagger.Schemas;

namespace Records.Shared.Contracts;

/// <summary>
/// Represents a validation error.
/// </summary>
public class ValidationErrorDto
{
    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationErrorDto"/> class.
    /// </summary>
    /// <param name="propertyName">The name of the property that have not pass the validation.</param>
    /// <param name="errorMessage">The validation description.</param>
    /// <param name="attemptedValue">The attempted value.</param>
    public ValidationErrorDto(string propertyName, string errorMessage, object attemptedValue)
    {
        PropertyName = propertyName;
        ErrorMessage = errorMessage;
        AttemptedValue = attemptedValue;
    }

    #endregion

    #region Properties

    /// <summary>The name of the property that have not pass the validation.</summary>
    [SwaggerSchemaExample("Country.IataCode")]
    public string PropertyName { get; }

    /// <summary>The validation description.</summary>
    [SwaggerSchemaExample("'IataCode' should not be empty.")]
    public string ErrorMessage { get; }

    /// <summary>The attempted value.</summary>
    [SwaggerSchemaExample("")]
    public object AttemptedValue { get; }

    #endregion
}
