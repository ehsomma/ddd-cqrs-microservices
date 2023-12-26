#region Usings

using Microsoft.AspNetCore.Http;
using My.Core.Exceptions;
using Records.Shared.Contracts;
using Serilog;
using System.Net;

#endregion

namespace Records.Shared.Infra.Http.Middlewares;

/// <summary>
/// Represents the global exception handler middleware that can be added to the application's request
/// pipeline.
/// </summary>
public class GlobalExceptionHandlerMiddleware : IMiddleware
{
    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="GlobalExceptionHandlerMiddleware"/> class.
    /// </summary>
    public GlobalExceptionHandlerMiddleware()
    {
        ////_next = next;
    }

    #endregion

    #region Public methods

    /// <inheritdoc />
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(next);

        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    #endregion

    #region Private methods

    /// <summary>
    /// Handles the specified <see cref="Exception"/> for the specified <see cref="HttpContext"/>.
    /// </summary>
    /// <param name="httpContext">The HTTP httpContext.</param>
    /// <param name="ex">The exception.</param>
    /// <returns>The HTTP response that is modified based on the exception.</returns>
    private async Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
    {
        string errorCode = ex.GetDataValue(ExDataKey.ErrorCode) ?? "ERR";
        string errorGroup = ex.GetDataValue(ExDataKey.ErrorGroup) ?? string.Empty;
        string? errorMessage = ex.Message;
        string? errorLogId = null;
        ex.SetTimeStamp(); // Solo la agrega si no tiene previamente.

        HttpStatusCode httpStatusCode = ResolveHttpStatusCode(errorCode, errorGroup);

        // If the httpStatusCode is 500, it should log.
        if (httpStatusCode == HttpStatusCode.InternalServerError)
        {
            errorLogId = Guid.NewGuid().ToString("N");

            // This is what we will see in the log (logId and the full exception) if the
            // exception should be logged.
            ex.Data[ExDataKey.ErrorLogId] = errorLogId;
            Log.Error(ex, "An exception occurred (id: {ErrorLogId}): {Message}", errorLogId, ex.Message);

            errorMessage = $"An exception occurred. Please, provide this error log id to technical support (logId: {errorLogId}).";
        }

        DateTime timeStampUtc = ex.GetTimeStamp();

        // This is what the user will see.
        ErrorResponse errorResponse = new (
            errorCode,
            errorMessage,
            (int)httpStatusCode,
            timeStampUtc,
            errorLogId,
            ValidationErrorsToDto(GetValidationErrors(ex)));

        // Prepare the response.
        string response = errorResponse.ToJson();
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = (int)httpStatusCode;

        await httpContext.Response.WriteAsync(response);
    }

    /// <summary>
    /// If the exception is ValidationException, gets the ValidationErros, otherwise return null.
    /// </summary>
    /// <param name="ex">The exception.</param>
    /// <returns>The validation erros.</returns>
    private IReadOnlyCollection<ValidationError>? GetValidationErrors(Exception ex)
    {
        return (ex is ValidationException)
            ? ((ValidationException)ex).ValidationErrors ?? null
            : null;
    }

    // Map IReadOnlyCollection<ValidationError> to

    /// <summary>
    /// Maps an IReadOnlyCollection of <see cref="ValidationError"/> to a IReadOnlyCollection of
    /// <see cref="ValidationErrorDto"/>.
    /// </summary>
    /// <param name="validationErrors">The ValidationErros.</param>
    /// <returns>The validation erros dtos.</returns>
    private IReadOnlyCollection<ValidationErrorDto>? ValidationErrorsToDto(IReadOnlyCollection<ValidationError>? validationErrors)
    {
        IReadOnlyCollection<ValidationErrorDto>? validationErrorsDto = null;

        if (validationErrors != null)
        {
            validationErrorsDto = validationErrors
                .Select(failure => new ValidationErrorDto(
                    failure.PropertyName,
                    failure.ErrorMessage,
                    failure.AttemptedValue))
                .ToList().AsReadOnly();
        }

        return validationErrorsDto;
    }

    /// <summary>
    /// Resolves the <see cref="HttpStatusCode"/> from the <paramref name="errorCode"/> or <paramref name="errorGroup"/> specified.
    /// </summary>
    /// <param name="errorCode">The custom errorCode (got from the exception).</param>
    /// <param name="errorGroup">The custom errorGroup (got from the exception).</param>
    /// <returns>The resolved <see cref="HttpStatusCode"/>.</returns>
    private HttpStatusCode ResolveHttpStatusCode(string errorCode, string errorGroup)
    {
        HttpStatusCode ret;

        if (string.IsNullOrWhiteSpace(errorCode))
        {
            ret = HttpStatusCode.InternalServerError; // 500.
        }
        else
        {
            if (string.IsNullOrWhiteSpace(errorGroup))
            {
                errorGroup = string.Empty;
            }

            if (errorCode == ExErrorCodeCore.ErrValidation
                || errorCode.Contains(".VAL.", StringComparison.InvariantCultureIgnoreCase)
                || errorCode.Contains(".VALIDATION", StringComparison.CurrentCultureIgnoreCase)
                || errorGroup == "Validation"
                || errorGroup == "DomainValidation")
            {
                ret = HttpStatusCode.BadRequest; // 400.
            }
            else if (errorCode.Contains(".AUTH.", StringComparison.InvariantCultureIgnoreCase)
                     || errorGroup == "Auth")
            {
                ret = HttpStatusCode.Unauthorized; // 401.
            }
            else if (errorCode.Contains(".FORB.", StringComparison.InvariantCultureIgnoreCase)
                || errorCode.Contains(".DOM.", StringComparison.InvariantCultureIgnoreCase)
                || errorCode.Contains(".BNS.", StringComparison.InvariantCultureIgnoreCase)
                || errorGroup == "Domain"
                || errorGroup == "Business")
            {
                ret = HttpStatusCode.Forbidden; // 403.
            }
            else
            {
                ret = HttpStatusCode.InternalServerError; // 500.
            }
        }

        return ret;
    }

    #endregion
}
