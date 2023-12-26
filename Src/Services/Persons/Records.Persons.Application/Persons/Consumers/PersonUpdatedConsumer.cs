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
/// Represents a MassTransit consumer of the message <see cref="Message{PersonUpdatedIntegrationEvent}"/>
/// (TContent:<see cref="PersonUpdatedIntegrationEvent"/>).
/// </summary>
public sealed class PersonUpdatedConsumer : ConsumerMetadataSupport<Message<PersonUpdatedIntegrationEvent>>
{
    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="PersonUpdatedConsumer"/> class.
    /// </summary>
    /// <param name="idempotentMessageService">Service to manage idempotent messages.</param>
    /// <param name="obsoleteMessageService">Service to prevent obsoletes messages.</param>
    public PersonUpdatedConsumer(
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
    public override async Task Consume(ConsumeContext<Message<PersonUpdatedIntegrationEvent>> context)
    {
        ArgumentNullException.ThrowIfNull(context);

        MessageMetadata messageMetadata = context.Message.Metadata;

        if (await NotYetConsumed(messageMetadata) && await NotObsolete(messageMetadata))
        {
            Log.Information($"[PersonUpdatedConsumer << PersonUpdatedIntegrationEvent] Message: {context.Message}");
        }
    }

    #endregion
}
