#pragma warning disable  // Disable all warnings (not used).

namespace Records.Shared.Infra.Http;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Records.Shared.Contracts;
using Records.Shared.Infra.Rop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


public static class ActionResultExtensions
{
    // For voids.
    public static IActionResult ToActionResult(this Result result)
    {
        // Cannot convert null literal to non-nullable reference type.
        Result<object> newResult = new Result<object>(null!, result.IsSuccess, result.Error);

        HttpStatusCode statusCode = ResolveHttpStatusCode(newResult);

        return newResult
            .ToDto(statusCode)
            .ToHttpStatusCode(statusCode);
    }

    public static IActionResult ToActionResult<T>(this Result<T> result)
    {
        HttpStatusCode statusCode = ResolveHttpStatusCode(result);

        return result
            .ToDto(statusCode)
            .ToHttpStatusCode(statusCode);
    }

    private static ResultDto<T> ToDto<T>(this Result<T> result, HttpStatusCode statusCode)
    {
        if (result.IsSuccess)
            return new ResultDto<T>()
            {
                StatusCode = (int)statusCode,
                Value = result.Value,
                Error = null
            };

        return new ResultDto<T>()
        {
            StatusCode = (int)statusCode,
            Value = default,

            Error = new ErrorDto(
                result.Error!.Code,
                result.Error.Message,
                result.Error.ErrorLogId)
        };
    }

    private static IActionResult ToHttpStatusCode<T>(this T resultDto, HttpStatusCode statusCode)
    {
        return new ResultWithStatusCode<T>(resultDto, statusCode);
    }

    private static HttpStatusCode ResolveHttpStatusCode<T>(Result<T> result)
    {
        HttpStatusCode ret;

        if (result.IsSuccess)
        {
            // It doesn't work, get "Error: response status is 204".
            //ret = result.Value == null ? HttpStatusCode.NoContent : HttpStatusCode.OK; // 204/200.
            ret = HttpStatusCode.OK; // 200.
        }
        else
        {
            string errorCode = result.Error!.Code;
            string errorGroup = result.Error!.Group;

            if (errorCode.Contains(".VAL.") || errorGroup == "Validation")
            {
                ret = HttpStatusCode.BadRequest; // 400.
            }
            else if (errorCode.Contains(".AUTH.") || errorGroup == "Auth")
            {
                ret = HttpStatusCode.Unauthorized; // 401.
            }
            else if (errorCode.Contains(".FORB.")
                || errorCode.Contains(".DOM.")
                || errorCode.Contains(".BNS.")
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

    private class ResultWithStatusCode<T> : ObjectResult
    {
        public ResultWithStatusCode(T content, HttpStatusCode httpStatusCode)
            : base(content)
        {
            StatusCode = (int)httpStatusCode;
        }
    }
}
#pragma warning restore  // Disable all warnings
