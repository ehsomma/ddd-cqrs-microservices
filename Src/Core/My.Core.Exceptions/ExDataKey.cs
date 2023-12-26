namespace My.Core.Exceptions;

/// <summary>
/// Keys for the Exception.Data dictionary.
/// </summary>
public static class ExDataKey
{
    #region Declarations

    /// <summary>Key that identifies the collection of validation errors.</summary>
    public const string ValidationErrors = "ValidationErrors";

    /// <summary>Key that identifies the unique error code of an exception (allows it to be shown to the user and to quickly locate the exception in the application log).</summary>
    public const string ErrorLogId = "ErrorLogId";

    /// <summary>Key that identifies the internal error code assigned from the business so that developers can make decisions according to this code.</summary>
    public const string ErrorCode = "ErrorCode";

    /// <summary>Key that identifies the error group (allows it to be associated with an http status code).</summary>
    public const string ErrorGroup = "ErrorGroup";

    /// <summary>Key that identifies the timeStamp.</summary>
    public const string ErrorTimeStamp = "ErrorTimeStamp";

    #endregion
}
