#region Usings

using MediatR;
using Records.Shared.Application.Messaging;
using Records.Shared.Domain.Events;
using Records.Shared.Infra.Persistence.Abstractions;
using Records.Shared.Messaging;
using System.Collections.ObjectModel;

#endregion

namespace Records.Shared.Application.Handlers;

/// <summary>
/// Represents the base command handler for a <see cref="ICommand"/> with void response.
/// </summary>
/// <typeparam name="TCommand">The command type to handle.</typeparam>
public abstract class CommandHandler<TCommand> : ICommandHandler<TCommand>
    where TCommand : ICommand
{
    #region Declarations

    /// <summary>Manage a <see cref="IDbSession"/> to encapsulate a business transaction which can affect the database.</summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.MaintainabilityRules",
        "SA1401:Fields should be private",
        Justification = "I prefer to use it as field just in protected fields in base classes like Repository base class.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Design",
        "CA1051:Do not declare visible instance fields",
        Justification = "I prefer to use it as field just in protected fields in base classes like Repository base class.")]
    //// ReSharper disable once InconsistentNaming
    protected readonly IUnitOfWork _unitOfWork; // From Persistence (not Projection).

    /// <summary>MediatR library to send and handle commands and queries implementing CQRS.</summary>
    private readonly IMediator _mediator;

    #endregion

    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandHandler{TCommand}"/> class.
    /// </summary>
    /// <param name="mediator">MediatR library to send and handle commands and queries implementing CQRS.</param>
    /// <param name="unitOfWork">Manage a <see cref="IDbSession"/> to encapsulate a business transaction which can affect the database.</param>
    /// <exception cref="ArgumentNullException">When some argument for the constructor parameters is null.</exception>
    protected CommandHandler(
        IMediator mediator,
        IUnitOfWork unitOfWork)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    #endregion

    #region Public methods

    /// <inheritdoc />
    public abstract Task Handle(TCommand command, CancellationToken cancellationToken);

    #endregion

    #region Private methods

    /// <summary>
    /// Publishes the specified domain events.
    /// </summary>
    /// <param name="domainEvents">The domain events to publish.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    protected async Task PublishDomainEvents(IReadOnlyCollection<IDomainEvent> domainEvents)
    {
        // NOTE: Publishes "different" events in parallel and also, by having MediatR configured with
        // TaskWhenAllPublisher(), if the same event has several different handlers, they are also
        // executed in parallel.
        ICollection<Task> tasks = new Collection<Task>();
        foreach (IDomainEvent domainEvent in domainEvents.NotNull())
        {
            // Build a generic Message<XxxxEvent> in runtime.
            object message = DomainMessage.Build(new MessageMetadata(domainEvent.AggregateId), domainEvent);
            tasks.Add(_mediator.Publish(message));
        }

        ////IEnumerable<Task> tasks = domainEvents.NotNull().Select(domainEvent =>
        ////{
        ////    // Build a generic Message<XxxxEvent> in runtime.
        ////    object message = DomainMessage.Build(new MessageMetadata(), domainEvent);
        ////    return _mediator.Publish(message);
        ////});

        await Task.WhenAll(tasks);
    }

    #endregion
}

/*
public abstract class CommandHandler<TCommand, TResponse> : ICommandHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
    private readonly IMediator _mediator;
    protected readonly IUnitOfWork _unitOfWork; // From Persistence (not Projection).

    protected CommandHandler(
        IMediator mediator,
        IUnitOfWork unitOfWork)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public abstract Task<TResponse> Handle(TCommand request, CancellationToken cancellationToken);

    protected async Task PublishDomainEvents(IReadOnlyCollection<IDomainEvent> domainEvents)
    {
        // NOTE: Publica "distintos" eventos en paralelo y además, al tener configurado MediatR con
        // TaskWhenAllPublisher(), si un mismo evento tiene varios handlers distintos, tambien
        // se ejecutan en paralelo.
        ICollection<Task> tasks = new Collection<Task>();
        foreach (IDomainEvent domainEvent in domainEvents.NotNull())
        {
            // Build a generic Message<XxxxEvent> in runtime.
            object message = DomainMessage.Build(new MessageMetadata(), domainEvent);
            tasks.Add(_mediator.Publish(message));
        }

        //IEnumerable<Task> tasks = domainEvents.NotNull().Select(domainEvent =>
        //{
        //    // Build a generic Message<XxxxEvent> in runtime.
        //    object message = DomainMessage.Build(new MessageMetadata(), domainEvent);
        //    return _mediator.Publish(message);
        //});

        await Task.WhenAll(tasks);
    }
}
*/
