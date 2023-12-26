using System.Text.Json;

namespace Records.Shared.Messaging;

/// <summary>
/// Represents a message that contain the data (e.g. Events) to be sent to a consumer or handler
/// (the Content) and all the metadata of the message <see cref="MessageMetadata"/>.
/// </summary>
/// <typeparam name="TContent">The type of the content of the message.</typeparam>
public class Message<TContent>
{
    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="Message{TContent}"/> class.
    /// </summary>
    /// <param name="metadata">The metadata of the Message.</param>
    /// <param name="content">The content of the Message.</param>
    /// <exception cref="ArgumentNullException">When some argument for the constructor parameters is null.</exception>
    public Message(MessageMetadata metadata, TContent content)
    {
        Metadata = metadata ?? throw new ArgumentNullException(nameof(metadata));
        Content = content ?? throw new ArgumentNullException(nameof(content));
    }

    #endregion

    #region Preperties

    /// <summary>The metadata of the Message.</summary>
    public MessageMetadata Metadata { get; init; }

    /// <summary>The content of the Message.</summary>
    public TContent Content { get; private set; }

    #endregion
}

/// <summary>
/// Represents a message that contain the data to be sent to a consumer or handler (the Content) and
/// all the metadata of the message <see cref="MessageMetadata"/>.
/// </summary>
public static class Message
{
    #region Public methods

    /// <summary>
    /// Create a generic instance of Message«TContent» in run-time.
    /// </summary>
    /// <remarks>
    /// When iterates on the IEnumerable of domain events, they are IIntegrationEvent and we need to
    /// send to .Publish() a specific Message«PersonUpdatedIntegrationEvent», not a Message«IIntegrationEvent».
    /// So in C# we can't create a generic in run-time like this:
    /// object myObject = new Message«integrationEvent.GetType()».
    /// </remarks>
    /// <typeparam name="TContent">The type of the content of the message.</typeparam>
    /// <param name="metadata">The metadata of the message.</param>
    /// <param name="content">The content of the message (e.g. a integration event).</param>
    /// <returns>A generic Message of TContent.</returns>
    public static object Build<TContent>(MessageMetadata metadata, TContent content)
    {
        ArgumentNullException.ThrowIfNull(metadata);

        object? message;

        if (content != null)
        {
            Type type = typeof(Message<>).MakeGenericType(content.GetType());

            message = Activator.CreateInstance(type, new object[] { metadata, content });

            if (message == null)
            {
                throw new NullReferenceException($"Cannot create generic instance Message of {content.GetType().Name}");
            }
        }
        else
        {
            throw new ArgumentNullException(nameof(content));
        }

        return message;
    }

    /// <summary>
    /// Create a generic instance of Message«TContent» in run-time from an OutboxMessage.
    /// </summary>
    /// <remarks>
    /// When iterates on the IEnumerable of domain events, they are IIntegrationEvent and we need to
    /// send to .Publish() a specific Message«PersonUpdatedIntegrationEvent», not a Message«IIntegrationEvent».
    /// So in C# we can't do something like:
    /// object myObject = new Message«integrationEvent.GetType()».
    /// </remarks>
    /// <param name="outboxMessage">The outboxMessage to convert to Message«TContent».</param>
    /// <returns>A generic Message of TContent.</returns>
    public static object Build(OutboxMessage outboxMessage)
    {
        ArgumentNullException.ThrowIfNull(outboxMessage);

        MessageMetadata metadata = new ()
        {
            MessageId = outboxMessage.MessageId,
            CorrelationId = outboxMessage.CorrelationId,
            CausationId = outboxMessage.CausationId,
            ContentId = outboxMessage.ContentId,
            CreatedOnUtc = outboxMessage.CreatedOnUtc,
            Host = outboxMessage.Host,
            Version = outboxMessage.Version,
        };

        // Deserializa el content al tipo contentType.
        Type contentType = Type.GetType(outboxMessage.ContentType) !;

        if (contentType == null)
        {
            throw new NullReferenceException($"Cannot get type {outboxMessage.ContentType} to deserialize the content of the message");
        }

        object content = JsonSerializer.Deserialize(outboxMessage.Content, contentType) !;

        // Create a generic instance of Message«TContent» in run - time.
        Type type = typeof(Message<>).MakeGenericType(content.GetType());
        object? message = Activator.CreateInstance(type, new object[] { metadata, content });

        if (message == null)
        {
            throw new NullReferenceException($"Cannot create generic instance Message of {content.GetType().Name}");
        }

        return message;
    }

    #endregion
}
