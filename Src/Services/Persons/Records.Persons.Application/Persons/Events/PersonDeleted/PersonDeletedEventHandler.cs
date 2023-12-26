#region Usings

using Records.Persons.Domain.Persons.Events;
using Records.Shared.Application.Messaging;
using Records.Shared.IntegrationEvents.Persons;
using Records.Shared.Messaging;
using Serilog;

#endregion

namespace Records.Persons.Application.Persons.Events.PersonDeleted;

/// <summary>
/// Represents the <see cref="DomainMessage{PersonDeletedEvent}"/> handler.
/// (TContent: <see cref="PersonDeletedEvent"/>).
/// </summary>
internal sealed class PersonDeletedEventHandler : IDomainEventHandler<DomainMessage<PersonDeletedEvent>>
{
    #region Declarations

    /// <summary>Manages the persistence operations of the messages into the Outbox.</summary>
    private readonly IOutboxRepository _outboxRepository;

    #endregion

    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="PersonDeletedEventHandler"/> class.
    /// </summary>
    /// <param name="outboxRepository">Manages the persistence operations of the messages into the Outbox.</param>
    /// <exception cref="ArgumentNullException">When some argument for the constructor parameters is null.</exception>
    public PersonDeletedEventHandler(IOutboxRepository outboxRepository)
    {
        _outboxRepository = outboxRepository ?? throw new ArgumentNullException(nameof(outboxRepository));
    }

    #endregion

    #region Public methods

    /// <inheritdoc />
    public async Task Handle(DomainMessage<PersonDeletedEvent> message, CancellationToken cancellationToken)
    {
        // Save the message with the integration event into the Outbox for further processing.
        PersonDeletedIntegrationEvent integrationEvent = new () { Id = message.Content.Person.Id };
        MessageMetadata metadata = new (message.Metadata);
        Message<PersonDeletedIntegrationEvent> integrationMessage = new (metadata, integrationEvent);

        await _outboxRepository.SaveAsync(integrationMessage);

        Log.Information("[PersonDeletedEventHandler << PersonDeletedEvent] Handled");
    }

    #endregion
}
