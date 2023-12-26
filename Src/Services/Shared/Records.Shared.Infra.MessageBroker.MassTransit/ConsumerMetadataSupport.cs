#region Usings

using MassTransit;
using Records.Shared.Infra.Idempotence.Abstractions;
using Records.Shared.Messaging;
using Serilog;
using System;
using System.Threading.Tasks;

#endregion

namespace Records.Shared.Infra.MessageBroker.MassTransit;

/// <summary>
/// Represents a MassTransit consumer base class with methods to check idempotency and obsolete messages
/// through your metadata.
/// </summary>
/// <typeparam name="TMessage">The type of the message to consume.</typeparam>
public abstract class ConsumerMetadataSupport<TMessage> : IConsumer<TMessage>
        where TMessage : class
{
    #region Declarations

    private readonly IIdempotentMessageService _idempotentMessageService;

    private readonly IObsoleteMessageService _obsoleteMessageService;

    private readonly string _messageTypeName;

    #endregion

    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="ConsumerMetadataSupport{TMessage}"/> class.
    /// </summary>
    /// <param name="idempotentMessageService">Service to manage idempotent messages.</param>
    /// <param name="obsoleteMessageService">Service to prevent obsoletes messages.</param>
    /// <exception cref="ArgumentNullException">When some argument for the constructor parameters is null.</exception>
    protected ConsumerMetadataSupport(IIdempotentMessageService idempotentMessageService, IObsoleteMessageService obsoleteMessageService)
    {
        _idempotentMessageService = idempotentMessageService ?? throw new ArgumentNullException(nameof(idempotentMessageService));
        _obsoleteMessageService = obsoleteMessageService ?? throw new ArgumentNullException(nameof(obsoleteMessageService));

        // Uses the type namespace/name of the class that inherit from this class.

        // Option 1.
        // Use .Name (class name without namespace) when consuming the event in a single project.
        // This must go in conjunction with the MassTransit/RabbitMQ endpoint name (queue) configuration
        // where you define whether or not the namespace is added to the endpoints.
        // _messageTypeName = this.GetType().Name; //this.GetType().FullName, see: cfg.ConfigureEndpoints
        // in MassTransit DependencyInjection class.

        // Option 2.
        // Use .FullName (namespace + class name) when you need to consume the same event in several
        // consumers from different projects (and the consumer has the same name).
        // This must go in conjunction with the MassTransit/RabbitMQ endpoint name (queues) configuration
        // where you define whether or not the namespace is added to the endpoints,
        // see: cfg.ConfigureEndpoints in MassTransit DependencyInjection class.
        _messageTypeName = GetType().FullName!; ////this.GetType().Name
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    public abstract Task Consume(ConsumeContext<TMessage> context);

    /// <summary>
    /// Checks if the message (by his metadata) has not consumed yet.
    /// </summary>
    /// <param name="messageMetadata">The message <see cref="MessageMetadata"/>.</param>
    /// <returns>True, if the message has not consumed. Otherwise, true.</returns>
    protected async Task<bool> NotYetConsumed(MessageMetadata messageMetadata)
    {
        ArgumentNullException.ThrowIfNull(messageMetadata);

        bool ret;
        string messageId = messageMetadata.MessageId.ToString();

        // Checks if the message already exist in the idempotency database/cache.
        if (!await _idempotentMessageService.HasBeenProcessed(messageId, _messageTypeName))
        {
            // Not yet consumed!

            // key: {productName}:{messageTypeName}:{messageId}
            // Example 1: records-idempotency:Records.Persons.BackgroundTasks.Consumers.PersonUpdatedConsumer:21433677-1a1e-4557-9d60-b3b2601a53f3
            // Example 2: records-idempotency:Records.Persons.Application.Consumers.PersonUpdatedConsumer:246eba3c-820f-4610-a7f8-644343562383
            // Persists it in the idempotency database/cache to avoid consume duplicated messages.
            await _idempotentMessageService.SaveAsync(messageId, _messageTypeName);
            ////_idempotentMessageService.SaveAsync(messageId, _messageTypeName);
            ret = true;
        }
        else
        {
            Log.Warning($"Message: '{_messageTypeName}:{messageId}' skipped (already processed)");

            ret = false;
        }

        return ret;
    }

    /// <summary>
    /// Checks if the message (by his metadata) is not obsolete (user in update operations).
    /// </summary>
    /// <param name="messageMetadata">The message <see cref="MessageMetadata"/>.</param>
    /// <returns>True, if the message is not obsolete. Otherwise, true.</returns>
    protected async Task<bool> NotObsolete(MessageMetadata messageMetadata)
    {
        ArgumentNullException.ThrowIfNull(messageMetadata);

        bool ret;
        DateTime messageCreatedOnUtc = messageMetadata.CreatedOnUtc;
        string messageContentId = messageMetadata.ContentId;

        // Checks if the message is obsolete.
        if (await _obsoleteMessageService.IsNotObsolete(_messageTypeName, messageCreatedOnUtc, messageContentId))
        {
            // Not obsolete!

            // key: {productName}:{messageTypeName}:{messageContentId}
            // Example 1: records-obsoletesPrevention:Records.Persons.BackgroundTasks.Consumers.PersonUpdatedConsumer:21433677-1a1e-4557-9d60-b3b2601a53f3
            // Example 2: records-obsoletesPrevention:Records.Persons.Application.Consumers.PersonUpdatedConsumer:246eba3c-820f-4610-a7f8-644343562383
            // Persists it in the database/cache to avoid consume obsoletes messages.
            await _obsoleteMessageService.SaveAsync(_messageTypeName, messageCreatedOnUtc, messageContentId);
            ////_obsoleteMessageService.SaveAsync(_messageTypeName, messageCreatedOnUtc, messageContentId);
            ret = true;
        }
        else
        {
            Log.Warning($"Message: '{_messageTypeName}:{messageContentId}' created on {messageCreatedOnUtc} skipped (obsolet)");

            ret = false;
        }

        return ret;
    }

    #endregion
}
