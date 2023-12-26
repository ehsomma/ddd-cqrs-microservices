#region Usings

using MassTransit;
using Records.Shared.Infra.Idempotence.Abstractions;
using Records.Shared.Infra.MessageBroker.MassTransit;
using Records.Shared.IntegrationEvents.Countries; // [!] Countries microservice.
using Records.Shared.Messaging;
using Serilog;

#endregion

namespace Records.Countries.Application.Countries.Consumers;

// NOTE: Unlike Records.Countries.BackgroundTasks, domain events can be consumed here via MessageBroker
// for asynchronous and/or retry-requiring tasks.

/// <summary>
/// Represents a MassTransit consumer of the message <see cref="Message{CountryCreatedIntegrationEvent}"/>
/// (TContent:<see cref="CountryCreatedIntegrationEvent"/>).
/// </summary>
public sealed class CountryCreatedConsumer : ConsumerMetadataSupport<Message<CountryCreatedIntegrationEvent>>
{
    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CountryCreatedConsumer"/> class.
    /// </summary>
    /// <param name="idempotentMessageService">Service to manage idempotent messages.</param>
    /// <param name="obsoleteMessageService">Service to prevent obsoletes messages.</param>
    public CountryCreatedConsumer(
        IIdempotentMessageService idempotentMessageService,
        IObsoleteMessageService obsoleteMessageService)
        : base(idempotentMessageService, obsoleteMessageService)
    {
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Consumes the message.
    /// </summary>
    /// <param name="context">Context that allow access to details surrounding the inbound message and its metadata, including headers.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public override async Task Consume(ConsumeContext<Message<CountryCreatedIntegrationEvent>> context)
    {
        ArgumentNullException.ThrowIfNull(context);

        if (await NotYetConsumed(context.Message.Metadata))
        {
            Log.Information($"[CountryCreatedConsumer << CountryCreatedIntegrationEvent] Message: {context.Message}");
        }
    }

    #endregion
}
