#pragma warning disable  // Disable all warnings (not used code)
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Records.Shared.Contracts;

public class ErrorDto

{
    public ErrorDto(string errorCode, string message, string errorLogId = "")
    {
        ErrorCode = errorCode;
        Message = message;

        if (string.IsNullOrWhiteSpace(errorLogId))
        {
            ErrorLogId = Guid.NewGuid().ToString("N");
        }
        else
        {
            ErrorLogId = errorLogId;
        }
    }

    public string ErrorCode { get; }

    public string? Message { get; }

    public string ErrorLogId { get; }
}
#pragma warning restore  // Disable all warnings
