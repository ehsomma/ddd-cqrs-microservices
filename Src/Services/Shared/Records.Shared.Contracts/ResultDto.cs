#pragma warning disable  // Disable all warnings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Records.Shared.Contracts;

public class ResultDto<T>
{
    public int StatusCode { get; set; }

    public T? Value { get; set; }

    public ErrorDto? Error { get; set; }

    public bool Success => this.Error == null;
}
#pragma warning restore  // Disable all warnings
