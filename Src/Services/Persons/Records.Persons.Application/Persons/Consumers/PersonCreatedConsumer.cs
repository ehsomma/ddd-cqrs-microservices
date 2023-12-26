#region Usings

using MassTransit;
using Records.Shared.Infra.Idempotence.Abstractions;
using Records.Shared.Infra.MessageBroker.MassTransit;
using Records.Shared.IntegrationEvents.Persons;
using Records.Shared.Messaging;
using Serilog;

#endregion

namespace Records.Persons.Application.Persons.Consumers;

// NOTE: Unlike Records.Persons.BackgroundTasks, domain events can be consumed here via MessageBroker
// for asynchronous and/or retry-requiring tasks.

/// <summary>
/// Represents a MassTransit consumer of the message <see cref="Message{PersonCreatedIntegrationEvent}"/>
/// (TContent:<see cref="PersonCreatedIntegrationEvent"/>).
/// </summary>
public sealed class PersonCreatedConsumer : ConsumerMetadataSupport<Message<PersonCreatedIntegrationEvent>>
{
    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="PersonCreatedConsumer"/> class.
    /// </summary>
    /// <param name="idempotentMessageService">Service to manage idempotent messages.</param>
    /// <param name="obsoleteMessageService">Service to prevent obsoletes messages.</param>
    public PersonCreatedConsumer(
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
    public override async Task Consume(ConsumeContext<Message<PersonCreatedIntegrationEvent>> context)
    {
        ArgumentNullException.ThrowIfNull(context);

        if (await NotYetConsumed(context.Message.Metadata))
        {
            Log.Information($"[PersonCreatedConsumer << PersonCreatedIntegrationEvent] Message: {context.Message}");
        }
    }

    #endregion
}
