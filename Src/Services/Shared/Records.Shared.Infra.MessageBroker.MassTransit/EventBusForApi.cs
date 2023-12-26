#region Usings

using MassTransit;
using Records.Shared.Infra.MessageBroker.Abstractions;
using System;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace Records.Shared.Infra.MessageBroker.MassTransit;

/// <summary>
/// Represents an EventBus actions to be used in APIs (uses IPublishEndpoint and ISendEndpointProvider).
/// </summary>
public sealed class EventBusForApi : IEventBus
{
    #region Declarations

    /// <summary>A publish endpoint lets the underlying transport determine the actual endpoint to which the message is sent. For example, an exchange on RabbitMQ and a topic on Azure Service bus.</summary>
    private readonly IPublishEndpoint _publishEndpoint;

    /// <summary>The Send Endpoint Provider is used to retrieve endpoints using addresses.</summary>
    private readonly ISendEndpointProvider _sendEndpointProvider;

    #endregion

    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="EventBusForApi"/> class.
    /// </summary>
    /// <param name="publishEndpoint">A publish endpoint lets the underlying transport determine the actual endpoint to which the message is sent. For example, an exchange on RabbitMQ and a topic on Azure Service bus.</param>
    /// <param name="sendEndpointProvider">The Send Endpoint Provider is used to retrieve endpoints using addresses.</param>
    public EventBusForApi(IPublishEndpoint publishEndpoint, ISendEndpointProvider sendEndpointProvider)
    {
        _publishEndpoint = publishEndpoint;
        _sendEndpointProvider = sendEndpointProvider;
    }

    #endregion

    #region Public methods

    /// <inheritdoc />
    public Task PublishAsync<T>(T message, CancellationToken cancellationToken = default)
        where T : class
    {
        return _publishEndpoint.Publish(message, cancellationToken);
    }

    /// <inheritdoc />
    public Task PublishAsync(object message, CancellationToken cancellationToken = default)
    {
        return _publishEndpoint.Publish(message, cancellationToken);
    }

    /// <inheritdoc />
    public async Task SendAsync<T>(Uri address, T message, CancellationToken cancellationToken = default)
        where T : class
    {
        ISendEndpoint endpoint = await _sendEndpointProvider.GetSendEndpoint(address);
        await endpoint.Send(message, cancellationToken);
    }

    #endregion
}
