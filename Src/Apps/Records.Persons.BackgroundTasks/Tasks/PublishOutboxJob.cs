#region Usings

using Quartz;
using Records.Shared.Infra.MessageBroker.Abstractions;
using Records.Shared.Infra.Quartz;
using Records.Shared.Messaging;
using Serilog;

#endregion

namespace Records.Persons.BackgroundTasks.Tasks;

/// <summary>
/// Represents a Job to read and publish the pending messages in the outbox.
/// </summary>
[DisallowConcurrentExecution]
public class PublishOutboxJob : JobTry<PublishOutboxJob>
{
    // Public key to reference in DI configuration.
    // Group helps to targeting specific jobs in maintenance operations, like pause all
    // jobs in group "integration".
    ////public static readonly JobKey Key = new JobKey(nameof(PublishOutboxJob), "group-1");

    #region Declarations

    /// <summary>EventBus to publish the message to all subscribed consumers.</summary>
    private readonly IEventBus _eventBus;

    /// <summary>Manages the persistence operations of the messages into the Outbox.</summary>
    private readonly IOutboxRepository _outboxRepository;

    #endregion

    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="PublishOutboxJob"/> class.
    /// </summary>
    /// <param name="eventBus">EventBus to publish the message to all subscribed consumers.</param>
    /// <param name="outboxRepository">Manages the persistence operations of the messages into the Outbox.</param>
    /// <exception cref="ArgumentNullException">When some argument for the constructor parameters is null.</exception>
    public PublishOutboxJob(
        IEventBus eventBus,
        IOutboxRepository outboxRepository)
    {
        _outboxRepository = outboxRepository ?? throw new ArgumentNullException(nameof(outboxRepository));
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
    }

    #endregion

    #region Public methods

    /// <inheritdoc />
    public override async Task TryExecute(IJobExecutionContext context)
    {
        CancellationTokenSource cancellationTokenSource = new ();
        CancellationToken cancellationToken = cancellationTokenSource.Token;

        IEnumerable<OutboxMessage>? outbox = await _outboxRepository.GetAllAsync();

        foreach (OutboxMessage outboxMessage in outbox!.NotNull())
        {
            try
            {
                object message = Message.Build(outboxMessage);

                // TODO: Add a Retry policy.
                await _eventBus.PublishAsync(message, cancellationToken);

                // TIP: (For eventBus) Use this way (casting to object) if you must iterate through
                // a list of IIntegrationEvent(its base) instead of a specific type like
                // PersonCreatedIntegrationEvent. This is so that MassTransit recognizes it at the
                // consumer.
                ////IIntegrationEvent integrationEvent = new PersonCreatedIntegrationEvent() { Id = notification.Person.Id, Name = notification.Person.FullName };
                ////this.eventBus.PublishAsync((object)integrationEvent, cancellationToken); // <===

                await _outboxRepository.PublishedAsync(outboxMessage.MessageId);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);

                await _outboxRepository.FailedAsync(outboxMessage.MessageId, ex.Message);

                // Absorbs the exception and continue with the next message.
            }
        }
    }

    #endregion
}
