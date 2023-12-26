#region Usings

using Records.Persons.Domain.Persons.Events;
using Records.Shared.Application.Messaging;
using Records.Shared.IntegrationEvents.Persons;
using Records.Shared.Messaging;
using Serilog;

#endregion

namespace Records.Persons.Application.Persons.Events.PersonCreated;

/// <summary>
/// Represents the <see cref="DomainMessage{PersonCreatedEvent}"/> handler.
/// (TContent: <see cref="PersonCreatedEvent"/>).
/// </summary>
internal sealed class PersonCreatedEventHandler : IDomainEventHandler<DomainMessage<PersonCreatedEvent>>
{
    #region Declarations

    /// <summary>Manages the persistence operations of the messages into the Outbox.</summary>
    private readonly IOutboxRepository _outboxRepository;

    #endregion

    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="PersonCreatedEventHandler"/> class.
    /// </summary>
    /// <param name="outboxRepository">Manages the persistence operations of the messages into the Outbox.</param>
    /// <exception cref="ArgumentNullException">When some argument for the constructor parameters is null.</exception>
    public PersonCreatedEventHandler(IOutboxRepository outboxRepository)
    {
        _outboxRepository = outboxRepository ?? throw new ArgumentNullException(nameof(outboxRepository));
    }

    #endregion

    #region Public methods

    /// <inheritdoc />
    public async Task Handle(DomainMessage<PersonCreatedEvent> message, CancellationToken cancellationToken)
    {
        // From here (event handler) you can:
        // • Publish integration events or save to the Outbox.
        // • Execute atomic logic (UOW/Transaction) idem command handlers.
        // • Create and send other commands.

        // From here (event handler) you can not:
        // • Execute other atomic logic of another aggregate.

        // Creates an integration event and saves it into the Outbox for further processing.
        PersonCreatedIntegrationEvent integrationEvent = new () { Id = message.Content.Person.Id, Name = message.Content.Person.FullName };
        MessageMetadata metadata = new (message.Metadata);
        Message<PersonCreatedIntegrationEvent> integrationMessage = new (metadata, integrationEvent);

        await _outboxRepository.SaveAsync(integrationMessage);

        Log.Information("[PersonCreatedEventHandler << PersonCreatedEvent] Handled");
    }

    #endregion
}
