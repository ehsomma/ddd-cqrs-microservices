#region Usings

using MassTransit;
using MediatR;
using Records.Persons.Application.Countries.Commands.CreateCountry;
using Records.Shared.Infra.Idempotence.Abstractions;
using Records.Shared.Infra.MessageBroker.MassTransit;
using Records.Shared.IntegrationEvents.Countries; // [!] Countries.CountryCreatedIntegrationEvent (from Countries microservice).
using Records.Shared.Messaging;
using Serilog;
using Dto = Records.Persons.Dtos.Countries; // Using aliases.

#endregion

namespace Records.Persons.Application.Countries.Consumers;

// NOTE: Unlike Records.Persons.BackgroundTasks, domain events can be consumed here via MessageBroker
// for asynchronous and/or retry-requiring tasks.

// Consume Countries.CountryCreatedIntegrationEvent, when a country was created in the
// Countries microservice (with full data). Then ("eventual consistency") fires a command
// to insert a Country (with partial data) in the Persons microservice.

/// <summary>
/// Represents a MassTransit consumer of the message <see cref="Message{CountryCreatedIntegrationEvent}"/>
/// (TContent:<see cref="CountryCreatedIntegrationEvent"/>).
/// </summary>
public sealed class CountryCreatedConsumer : ConsumerMetadataSupport<Message<CountryCreatedIntegrationEvent>>
{
    #region Declarations

    private readonly IMediator _mediator;

    #endregion

    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CountryCreatedConsumer"/> class.
    /// </summary>
    /// <param name="idempotentMessageService">Service to manage idempotent messages.</param>
    /// <param name="obsoleteMessageService">Service to prevent obsoletes messages.</param>
    /// <param name="mediator">MediatR library to send and handle commands and queries implementing CQRS.</param>
    /// <exception cref="ArgumentNullException">When some argument for the constructor parameters is null.</exception>
    public CountryCreatedConsumer(
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
    public override async Task Consume(ConsumeContext<Message<CountryCreatedIntegrationEvent>> context)
    {
        ArgumentNullException.ThrowIfNull(context);

        if (await NotYetConsumed(context.Message.Metadata))
        {
            // Create and send the CreateCountryCommand.
            CountryCreatedIntegrationEvent messageContent = context.Message.Content;
            Dto.Country countryDto = new () { IataCode = messageContent.IataCode, Name = messageContent.Name };
            CreateCountryCommand command = new (string.Empty, countryDto);

            await _mediator.Send(command);

            Log.Information($"[CountryCreatedConsumer << CountryCreatedIntegrationEvent] Message: {context.Message}");
        }
    }

    #endregion
}
