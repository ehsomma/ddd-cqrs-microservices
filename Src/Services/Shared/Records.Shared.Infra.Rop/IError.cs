#pragma warning disable  // Disable all warnings

namespace Records.Shared.Infra.Rop;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IError
{
    string Code { get; }

    string Group { get; }

    string Message { get; }

    string ErrorLogId { get; }
}

#pragma warning restore  // Restore all warnings
