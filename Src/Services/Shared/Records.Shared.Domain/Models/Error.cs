namespace Records.Shared.Domain.Models;

/// <summary>
/// Represents a concrete domain error.
/// </summary>
public sealed class Error
{
    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="Error"/> class.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="message">The error message.</param>
    /// <param name="group">The group of the message (helps to resolve the HttpStatusCode) in http requests.</param>
    public Error(string code, string message = "", string group = "")
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new ArgumentNullException(nameof(code));
        }

        Code = code;
        Message = message;
        Group = group;
    }

    #endregion

    #region Public methods

    /// <summary>Gets the error code.</summary>
    public string Code { get; }

    /// <summary>Gets the error message.</summary>
    public string Message { get; }

    /// <summary>The group of the message (helps to resolve the HttpStatusCode) in http requests.</summary>
    public string Group { get; }

    /// <summary>
    /// Implicit operator.
    /// </summary>
    /// <param name="error">The <see cref="Error"/>.</param>
    public static implicit operator string(Error error)
    {
        ArgumentNullException.ThrowIfNull(error);

        return error.Code;
    }

    #endregion
}
