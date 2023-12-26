#region Usings

using Records.Persons.Domain.Countries.Events;
using Records.Shared.Application.Messaging;
////using Records.Shared.IntegrationEvents.Countries; // [!] Countries
using Records.Shared.IntegrationEvents.Persons; // [!] Persons
using Records.Shared.Messaging;
using Serilog;
using DomainModel = Records.Persons.Domain.Countries.Models;

#endregion

namespace Records.Persons.Application.Countries.Events.CountryCreated;

/// <summary>
/// Represents the <see cref="DomainMessage{CountryCreatedEvent}"/> handler.
/// (TContent: <see cref="CountryCreatedEvent"/>).
/// </summary>
internal sealed class CountryCreatedEventHandler : IDomainEventHandler<DomainMessage<CountryCreatedEvent>>
{
    #region Declarations

    private readonly IOutboxRepository _outboxRepository;

    #endregion

    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CountryCreatedEventHandler"/> class.
    /// </summary>
    /// <param name="outboxRepository">Manages the persistence operations of the messages into the Outbox.</param>
    /// <exception cref="ArgumentNullException">When some argument for the constructor parameters is null.</exception>
    public CountryCreatedEventHandler(IOutboxRepository outboxRepository)
    {
        _outboxRepository = outboxRepository ?? throw new ArgumentNullException(nameof(outboxRepository));
    }

    #endregion

    #region Public methods

    /// <inheritdoc />
    public async Task Handle(DomainMessage<CountryCreatedEvent> message, CancellationToken cancellationToken)
    {
        // Save the message with the integration event into the Outbox for further processing.
        DomainModel.Country domainCountry = message.Content.Country;
        CountryCreatedIntegrationEvent integrationEvent = new (domainCountry.IataCode, domainCountry.Name);
        MessageMetadata metadata = new (message.Metadata);
        Message<CountryCreatedIntegrationEvent> integrationMessage = new (metadata, integrationEvent);

        await _outboxRepository.SaveAsync(integrationMessage);

        Log.Information("[CountryCreatedEventHandler << CountryCreatedEvent] Handled");
    }

    #endregion
}
