#region Usings

using MassTransit;
using Records.Shared.Infra.MessageBroker.Abstractions;
using System;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace Records.Shared.Infra.MessageBroker.MassTransit;

/// <summary>
/// Represents an EventBus actions to be used in Workers (uses IBus).
/// </summary>
public sealed class EventBusForWorker : IEventBus
{
    #region Contructor

    /// <summary>A bus is a logical element that includes a local endpoint and zero or more receive endpoints.</summary>
    private readonly IBus _bus;

    #endregion

    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="EventBusForWorker"/> class.
    /// </summary>
    /// <param name="bus">A bus is a logical element that includes a local endpoint and zero or more receive endpoints.</param>
    public EventBusForWorker(IBus bus)
    {
        _bus = bus;
    }

    #endregion

    #region Public methods

    /// <inheritdoc />
    public Task PublishAsync<T>(T message, CancellationToken cancellationToken = default)
        where T : class
    {
        return _bus.Publish(message, cancellationToken);
    }

    /// <inheritdoc />
    public Task PublishAsync(object message, CancellationToken cancellationToken = default)
    {
        return _bus.Publish(message, cancellationToken);
    }

    /// <inheritdoc />
    public async Task SendAsync<T>(Uri address, T message, CancellationToken cancellationToken = default)
        where T : class
    {
        ISendEndpoint endpoint = await _bus.GetSendEndpoint(address);
        await endpoint.Send(message, cancellationToken);
    }

    #endregion
}
