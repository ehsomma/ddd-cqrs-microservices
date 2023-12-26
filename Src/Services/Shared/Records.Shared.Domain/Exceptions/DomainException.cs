#region Usings

using My.Core.Exceptions;
using Records.Shared.Domain.Models;

#endregion

namespace Records.Shared.Domain.Exceptions;

/// <summary>
/// Represents an exception that occurs in the domain and contains information about the domain <see cref="Records.Shared.Domain.Models.Error"/>.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Design",
    "CA1032:Implement standard exception constructors",
    Justification = "We want to use this constructor only.")]
public class DomainException : Exception
{
    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="DomainException"/> class.
    /// </summary>
    /// <param name="error">The error containing the information about what happened.</param>
    public DomainException(Error error)
        : base(error.Message)
    {
        Error = error;

        Data[ExDataKey.ErrorCode] = error.Code;
        Data[ExDataKey.ErrorGroup] = error.Group;
    }

    #endregion

    #region Properties

    /// <summary>Gets the error.</summary>
    public Error Error { get; }

    #endregion
}
