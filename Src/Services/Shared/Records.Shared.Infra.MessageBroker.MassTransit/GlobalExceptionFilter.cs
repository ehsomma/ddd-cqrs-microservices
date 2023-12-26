#region Usings

using MassTransit;
using Serilog;
using System;
using System.Threading.Tasks;

#endregion

namespace Records.Shared.Infra.MessageBroker.MassTransit;

/// <inheritdoc />
public class GlobalExceptionFilter<T> : IFilter<ConsumeContext<T>>
    where T : class
{
    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="GlobalExceptionFilter{T}"/> class.
    /// </summary>
    public GlobalExceptionFilter()
    {
    }

    #endregion

    #region Public methods

    /// <inheritdoc />
    public async Task Send(ConsumeContext<T> context, IPipe<ConsumeContext<T>> next)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(next);

        try
        {
            // Ejecuta el siguiente paso en la cadena de filtros.
            await next.Send(context);
        }
        catch (Exception ex)
        {
            ex.Data["Message"] = context.Message;

            Log.Error(ex, $"Exception in consummer: {ex.Message}");

            // Propagate the exception up the call stack.
            throw;
        }
    }

    /// <inheritdoc />
    public void Probe(ProbeContext context)
    {
    }

    #endregion
}
