#region Usings

using MediatR;
using Records.Shared.Messaging;

#endregion

namespace Records.Shared.Application.Messaging;

/// <summary>
/// Represents a <see cref="Message{TContent}"/> but implementing <see cref="MediatR.INotification"/> to be
/// able to handle it with MadiatR library.
/// </summary>
/// <typeparam name="TContent">The type of the content of the message.</typeparam>
public class DomainMessage<TContent> : Message<TContent>, INotification
{
    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="DomainMessage{TContent}"/> class.
    /// </summary>
    /// <param name="metadata">The metadata of the Message.</param>
    /// <param name="content">The content of the Message.</param>
    public DomainMessage(MessageMetadata metadata, TContent content)
        : base(metadata, content)
    {
    }

    #endregion
}

/// <summary>
/// Represents a <see cref="Message"/> but implementing <see cref="MediatR.INotification"/> to be
/// able to handle it with MadiatR library.
/// </summary>
public static class DomainMessage
{
    #region Public methods

    /// <summary>
    /// Create a generic instance of DomainMessage«TContent» in run-time.
    /// </summary>
    /// <remarks>
    /// When iterates on the IEnumerable of domain events, they are IDomainEvent and we need to
    /// send to .Publish() a specific DomainMessage«PersonUpdatedEvent», not a DomainMessage«IDomainEvent».
    /// So in C# we can't do something like:
    /// object myObject = new DomainMessage«domainEvent.GetType().
    /// </remarks>
    /// <typeparam name="TContent">The type of the content of the message.</typeparam>
    /// <param name="metadata">The metadata of the message.</param>
    /// <param name="content">The content of the message (e.g. a domain event).</param>
    /// <returns>A generic DomainMessage of TContent.</returns>
    public static object Build<TContent>(MessageMetadata metadata, TContent content)
    {
        object? message;

        if (content != null)
        {
            Type type = typeof(DomainMessage<>).MakeGenericType(content.GetType());

            message = Activator.CreateInstance(type, new object[] { metadata, content });

            if (message == null)
            {
                throw new NullReferenceException($"Cannot create generic instance DomainMessage of {content.GetType().Name}");
            }
        }
        else
        {
            throw new ArgumentNullException(nameof(content));
        }

        return message;
    }

    #endregion
}
