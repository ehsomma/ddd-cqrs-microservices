namespace My.Core.Exceptions;

/// <summary>
/// Extensions methods for exceptions.
/// </summary>
public static class ExceptionExtensions
{
    #region Methods

    /// <summary>Add an errorCode to the exception.</summary>
    /// <param name="ex">The exception.</param>
    /// <param name="exceptionErrorCode">The error code to add. (Use class ExceptionErrorCode).</param>
    /// <returns>The <see cref="Exception"/>.</returns>
    public static Exception AddErrorCode(this Exception ex, string exceptionErrorCode)
    {
        ArgumentNullException.ThrowIfNull(ex);

        ex.Data[ExDataKey.ErrorCode] = exceptionErrorCode;

        return ex;
    }

    /// <summary>Get an errorCode from the data dictionary of the exception.</summary>
    /// <param name="ex">The exception from where to find.</param>
    /// <returns>The ErrorCode. If not found, null.</returns>
    public static string? GetErrorCode(this Exception ex)
    {
        ArgumentNullException.ThrowIfNull(ex);

        string? ret = ex.Data[ExDataKey.ErrorCode]?.ToString();

        return ret;
    }

    /// <summary>
    /// Tries to get the error code (previously set) from the exception data object. Recursively
    /// searches internal exceptions.
    /// </summary>
    /// <param name="ex">The exception where to search the error code.</param>
    /// <returns>The error code. If not found, null.</returns>
    public static string? GetErrorLogId(this Exception ex)
    {
        ArgumentNullException.ThrowIfNull(ex);

        string? errorLogId = null;

        if (ex != null)
        {
            if (ex.Data.Contains(ExDataKey.ErrorLogId))
            {
                errorLogId = ex.Data[ExDataKey.ErrorLogId]?.ToString();
            }
            else
            {
                errorLogId = ex.InnerException?.GetErrorLogId();
            }
        }

        return errorLogId;
    }

    /// <summary>
    /// Searches and obtains the value of the specified <paramref name="key"/> in the exception data object.
    /// </summary>
    /// <param name="ex">The exception where to search the key.</param>
    /// <param name="key">The key to search.</param>
    /// <returns>The value of the specified <paramref name="key"/>. If not found, null.</returns>
    public static string? GetDataValue(this Exception ex, string key)
    {
        ArgumentNullException.ThrowIfNull(ex);

        string? ret = ex.Data[key]?.ToString();

        return ret;
    }

    /// <summary>
    /// Adds a timestamp key/value to the exception data object.
    /// </summary>
    /// <param name="ex">The exception where to set the timestamp.</param>
    /// <returns>The same exception.</returns>
    public static Exception SetTimeStamp(this Exception ex)
    {
        ArgumentNullException.ThrowIfNull(ex);

        if (!ex.Data.Contains(ExDataKey.ErrorTimeStamp))
        {
            ex.Data[ExDataKey.ErrorTimeStamp] = DateTime.UtcNow;
        }

        return ex;
    }

    /// <summary>
    /// Gets the timestamp from the data object of the exception.
    /// </summary>
    /// <param name="ex">The exception where to search the timestamp.</param>
    /// <returns>The same exception.</returns>
    public static DateTime GetTimeStamp(this Exception ex)
    {
        ArgumentNullException.ThrowIfNull(ex);

        DateTime ret;

        if (ex.Data.Contains(ExDataKey.ErrorTimeStamp))
        {
            if (DateTime.TryParse(ex.Data[ExDataKey.ErrorTimeStamp]?.ToString(), out DateTime timeStamp))
            {
                ret = timeStamp;
            }
            else
            {
                // If it have not a valid date, sets a new valid date.
                ret = DateTime.UtcNow;
                ex.Data[ExDataKey.ErrorTimeStamp] = ret;
            }
        }
        else
        {
            // If it does not have it, adds it.
            ret = DateTime.UtcNow;
            ex.Data[ExDataKey.ErrorTimeStamp] = ret;
        }

        return ret;
    }

    #endregion
}
