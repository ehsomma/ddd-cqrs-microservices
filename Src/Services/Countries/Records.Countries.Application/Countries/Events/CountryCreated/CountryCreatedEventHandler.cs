#region Usings

using Records.Countries.Domain.Countries.Events;
using Records.Shared.Application.Messaging;
using Records.Shared.IntegrationEvents.Countries;
using Records.Shared.Messaging;
using Serilog;

#endregion

namespace Records.Countries.Application.Countries.Events.CountryCreated;

/// <summary>
/// Represents the <see cref="DomainMessage{CountryCreatedEvent}"/> handler.
/// (TContent: <see cref="CountryCreatedEvent"/>).
/// </summary>
internal sealed class CountryCreatedEventHandler : IDomainEventHandler<DomainMessage<CountryCreatedEvent>>
{
    #region Declarations

    /// <summary>Manages the persistence operations of the messages into the Outbox.</summary>
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
        // TODO: Implement business logic.

        CountryCreatedIntegrationEvent integrationEvent = new (message.Content.Country.IataCode, message.Content.Country.Name);
        MessageMetadata metadata = new (message.Metadata);
        Message<CountryCreatedIntegrationEvent> integrationMessage = new (metadata, integrationEvent);

        await _outboxRepository.SaveAsync(integrationMessage);

        Log.Information("CountryCreatedEventHandler << CountryCreatedEvent Handled");
    }

    #endregion
}
