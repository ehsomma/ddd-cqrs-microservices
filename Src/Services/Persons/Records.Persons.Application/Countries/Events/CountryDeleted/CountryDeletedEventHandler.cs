﻿#region Usings

using Records.Persons.Domain.Countries.Events;
////using DomainModel = Records.Persons.Domain.Countries.Models;
using Records.Shared.Application.Messaging;
////using Records.Shared.IntegrationEvents.Countries; // [!] Countries
using Records.Shared.IntegrationEvents.Persons; // [!] Persons
using Records.Shared.Messaging;
using Serilog;

#endregion

namespace Records.Persons.Application.Countries.Events.CountryDeleted;

/// <summary>
/// Represents the <see cref="DomainMessage{CountryDeletedEvent}"/> handler.
/// (TContent: <see cref="CountryDeletedEvent"/>).
/// </summary>
internal sealed class CountryDeletedEventHandler : IDomainEventHandler<DomainMessage<CountryDeletedEvent>>
{
    #region Declarations

    /// <summary>Manages the persistence operations of the messages into the Outbox.</summary>
    private readonly IOutboxRepository _outboxRepository;

    #endregion

    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CountryDeletedEventHandler"/> class.
    /// </summary>
    /// <param name="outboxRepository">Manages the persistence operations of the messages into the Outbox.</param>
    /// <exception cref="ArgumentNullException">When some argument for the constructor parameters is null.</exception>
    public CountryDeletedEventHandler(IOutboxRepository outboxRepository)
    {
        _outboxRepository = outboxRepository ?? throw new ArgumentNullException(nameof(outboxRepository));
    }

    #endregion

    #region Public methods

    /// <inheritdoc />
    public async Task Handle(DomainMessage<CountryDeletedEvent> message, CancellationToken cancellationToken)
    {
        // Save the message with the integration event into the Outbox for further processing.
        CountryDeletedIntegrationEvent integrationEvent = new (message.Content.Country.IataCode);
        MessageMetadata metadata = new (message.Metadata);
        Message<CountryDeletedIntegrationEvent> integrationMessage = new (metadata, integrationEvent);

        await _outboxRepository.SaveAsync(integrationMessage);

        Log.Information("[CountryDeletedEventHandler << CountryDeletedEvent] Handled");
    }

    #endregion
}
