namespace My.Core.Exceptions;

/// <summary>
/// Represents a validation error.
/// </summary>
public class ValidationError
{
    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationError"/> class.
    /// </summary>
    /// <param name="propertyName">The name of the property that have not pass the validation.</param>
    /// <param name="errorMessage">The validation description.</param>
    public ValidationError(string propertyName, string errorMessage)
    {
        PropertyName = propertyName;
        ErrorMessage = errorMessage;
        AttemptedValue = string.Empty;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationError"/> class.
    /// </summary>
    /// <param name="propertyName">The name of the property that have not pass the validation.</param>
    /// <param name="errorMessage">The validation description.</param>
    /// <param name="attemptedValue">The attempted value.</param>
    public ValidationError(string propertyName, string errorMessage, object attemptedValue)
    {
        PropertyName = propertyName;
        ErrorMessage = errorMessage;
        AttemptedValue = attemptedValue;
    }

    #endregion

    #region Properties

    /// <summary>The name of the property that have not pass the validation.</summary>
    public string PropertyName { get; }

    /// <summary>The validation description.</summary>
    public string ErrorMessage { get; }

    /// <summary>The attempted value.</summary>
    public object AttemptedValue { get; }

    #endregion
}
