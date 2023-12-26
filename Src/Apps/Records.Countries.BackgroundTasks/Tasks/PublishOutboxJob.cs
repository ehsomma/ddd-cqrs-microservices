#region Usings

using Quartz;
using Records.Shared.Infra.MessageBroker.Abstractions;
using Records.Shared.Infra.Quartz;
using Records.Shared.Messaging;

using Serilog;

#endregion

namespace Records.Countries.BackgroundTasks.Tasks;

/// <summary>
/// Represents a Job to read and publish the pending messages in the outbox.
/// </summary>
[DisallowConcurrentExecution]
public class PublishOutboxJob : JobTry<PublishOutboxJob>
{
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

        ////int count = 0;
        foreach (OutboxMessage outboxMessage in outbox!.NotNull())
        {
            try
            {
                object message = Message.Build(outboxMessage);

                // TODO: Add a Retry policy.
                await _eventBus.PublishAsync(message, cancellationToken);

                await _outboxRepository.PublishedAsync(outboxMessage.MessageId);

                ////count++;
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
