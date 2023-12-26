#region Usings

using Records.Persons.Domain.Persons.Events;
using Records.Shared.Application.Messaging;
using Records.Shared.IntegrationEvents.Persons;
using Records.Shared.Messaging;
using Serilog;

#endregion

namespace Records.Persons.Application.Persons.Events.PersonUpdated;

/// <summary>
/// Represents the <see cref="DomainMessage{PersonUpdatedEvent}"/> handler.
/// (TContent: <see cref="PersonUpdatedEvent"/>).
/// </summary>
internal sealed class PersonUpdatedEventHandler : IDomainEventHandler<DomainMessage<PersonUpdatedEvent>>
{
    #region Declarations

    /// <summary>Manages the persistence operations of the messages into the Outbox.</summary>
    private readonly IOutboxRepository _outboxRepository;

    #endregion

    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="PersonUpdatedEventHandler"/> class.
    /// </summary>
    /// <param name="outboxRepository">Manages the persistence operations of the messages into the Outbox.</param>
    /// <exception cref="ArgumentNullException">When some argument for the constructor parameters is null.</exception>
    public PersonUpdatedEventHandler(IOutboxRepository outboxRepository)
    {
        _outboxRepository = outboxRepository ?? throw new ArgumentNullException(nameof(outboxRepository));
    }

    #endregion

    #region Public methods

    /// <inheritdoc />
    public async Task Handle(DomainMessage<PersonUpdatedEvent> message, CancellationToken cancellationToken)
    {
        // Save the message with the integration event into the Outbox for further processing.
        MessageMetadata metadata = new (message.Metadata);
        PersonUpdatedIntegrationEvent integrationEvent = new () { Id = message.Content.Person.Id, Name = message.Content.Person.FullName };
        Message<PersonUpdatedIntegrationEvent> integrationMessage = new (metadata, integrationEvent);

        await _outboxRepository.SaveAsync(integrationMessage);

        Log.Information("[PersonUpdatedEventHandler << PersonUpdatedEvent] Handled");
    }

    #endregion
}
