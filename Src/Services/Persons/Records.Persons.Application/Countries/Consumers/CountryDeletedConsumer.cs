#region Usings

using MassTransit;
using MediatR;
using Records.Persons.Application.Countries.Commands.DeleteCountry;
using Records.Shared.Infra.Idempotence.Abstractions;
using Records.Shared.Infra.MessageBroker.MassTransit;
using Records.Shared.IntegrationEvents.Countries; // [!] Countries.CountryDeletedIntegrationEvent (from Countries microservice).
using Records.Shared.Messaging;
////using Dto = Records.Persons.Dtos.Countries; // Using aliases.
using Serilog;

#endregion

// Consume Countries.CountryDeletedIntegrationEvent, when a country was deleted in the Countries
namespace Records.Persons.Application.Countries.Consumers;

// microservice. Then, with "eventual consistency", fires a command to delete a Country in
// the Persons microservice.

// NOTE: Unlike Records.Persons.BackgroundTasks, domain events can be consumed here via MessageBroker
// for asynchronous and/or retry-requiring tasks.

/// <summary>
/// Represents a MassTransit consumer of the message <see cref="Message{CountryDeletedIntegrationEvent}"/>
/// (TContent:<see cref="CountryDeletedIntegrationEvent"/>).
/// </summary>
public sealed class CountryDeletedConsumer : ConsumerMetadataSupport<Message<CountryDeletedIntegrationEvent>>
{
    #region Declarations

    private readonly IMediator _mediator;

    #endregion

    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CountryDeletedConsumer"/> class.
    /// </summary>
    /// <param name="idempotentMessageService">Service to manage idempotent messages.</param>
    /// <param name="obsoleteMessageService">Service to prevent obsoletes messages.</param>
    /// <param name="mediator">MediatR library to send and handle commands and queries implementing CQRS.</param>
    /// <exception cref="ArgumentNullException">When some argument for the constructor parameters is null.</exception>
    public CountryDeletedConsumer(
        IIdempotentMessageService idempotentMessageService,
        IObsoleteMessageService obsoleteMessageService,
        IMediator mediator)
        : base(idempotentMessageService, obsoleteMessageService)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Consumes the message.
    /// </summary>
    /// <param name="context">Context that allow access to details surrounding the inbound message and its metadata, including headers.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public override async Task Consume(ConsumeContext<Message<CountryDeletedIntegrationEvent>> context)
    {
        ArgumentNullException.ThrowIfNull(context);

        if (await NotYetConsumed(context.Message.Metadata))
        {
            // Creates and send DeleteCountryCommand.
            DeleteCountryCommand command = new (string.Empty, context.Message.Content.IataCode);

            await _mediator.Send(command);

            Log.Information($"[CountryDeletedConsumer << CountryDeletedIntegrationEvent] Message: {context.Message}");
        }
    }

    #endregion
}
