#region Usings

using Records.Shared.Infra.Swagger.Schemas;
using System;
using System.Collections.Generic;

#endregion

namespace Records.Shared.Contracts;

/// <summary>
/// Represents an error response when an http operation fails either for an internal error or a
/// business exception.
/// </summary>
public class ErrorResponse
{
    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="ErrorResponse"/> class.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="message">The error description.</param>
    /// <param name="httpSatusCode">The http status code.</param>
    /// <param name="timeStampUtc">The timestamp when the error was produced.</param>
    /// <param name="logId">An identifier of the error log so that it can be sent to technical support and with this the log of the complete exception can be searched (for internal errors).</param>
    /// <param name="validationErrors">A list of validation errors description (for validation errors).</param>
    public ErrorResponse(
        string code,
        string? message,
        int httpSatusCode,
        DateTime timeStampUtc,
        string? logId,
        IReadOnlyCollection<ValidationErrorDto>? validationErrors = null)
    {
        TimeStampUtc = DateTime.UtcNow;
        Code = code;
        Message = message;
        HttpStatusCode = httpSatusCode;
        TimeStampUtc = timeStampUtc;
        LogId = logId;
        ValidationErrors = validationErrors;
    }

    #endregion

    #region Properties

    /// <summary>The timestamp when the error was produced (UTC).</summary>
    public DateTime TimeStampUtc { get; }

    /// <summary>The error description.</summary>
    [SwaggerSchemaExample("ERR.VALIDATION")]
    public string Code { get; }

    /// <summary>The http status code.</summary>
    [SwaggerSchemaExample("400")]
    public int HttpStatusCode { get; }

    /// <summary>The error code.</summary>
    [SwaggerSchemaExample("Validation failed. See 'ValidationErrors' for more details.")]
    public string? Message { get; }

    /// <summary>
    /// An identifier of the error log so that it can be sent to technical support and with this the
    /// log of the complete exception can be searched (for internal errors).
    /// </summary>
    [SwaggerSchemaExample("5B5969D3-3A90-476B-ACF1-563FA849CB7D")]
    public string? LogId { get; }

    /// <summary>A list of validation errors description (for validation errors).</summary>
    public IReadOnlyCollection<ValidationErrorDto>? ValidationErrors { get; }

    #endregion
}
