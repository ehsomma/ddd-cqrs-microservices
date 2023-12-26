using My.Core.Exceptions;

namespace Records.Shared.Domain.Exceptions;

/// <summary>
/// Base exception to represent validations domain errors.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Design",
    "CA1032:Implement standard exception constructors",
    Justification = "We want to use this constructor only.")]
public class DomainValidationException : ValidationException
{
    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="DomainException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    public DomainValidationException(string message)
        : base(message)
    {
        SetDefaultErrorGroup();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DomainException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="innerException">The inner exception.</param>
    public DomainValidationException(string message, Exception innerException)
        : base(message, innerException)
    {
        SetDefaultErrorGroup();
    }

    #endregion

    #region Methods

    /// <summary>
    /// Set the erroGroup for validations.
    /// </summary>
    private void SetDefaultErrorGroup()
    {
        // The ExDataKey.ErrorCode is set in the base class.
        Data[ExDataKey.ErrorGroup] = "DomainValidation";
    }

    #endregion
}
