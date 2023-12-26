#region Usings

using MassTransit;
using Records.Persons.Projection.Countries.Projectors;
using Records.Shared.Infra.Idempotence.Abstractions;
using Records.Shared.Infra.MessageBroker.MassTransit;
using Records.Shared.IntegrationEvents.Persons; // [!] Fired from Persons microservice.
using Records.Shared.Messaging;
using Records.Shared.Projection.Abstractions;
using Serilog;

#endregion

namespace Records.Persons.BackgroundTasks.Consumers;

/// <summary>
/// Represents a MassTransit consumer of the message <see cref="Message{CountryCreatedIntegrationEvent}"/>
/// (TContent:<see cref="CountryCreatedIntegrationEvent"/>).
/// </summary>
public sealed class CountryCreatedConsumer : ConsumerMetadataSupport<Message<CountryCreatedIntegrationEvent>>
{
    #region Declarations

    /// <summary>Contains a session to encapsulate a business transaction which can affect the database.</summary>
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>Manages proyection operations from "source" database to "projection" (read) database.</summary>
    private readonly ICountryCrearedProjector _projector;

    #endregion

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CountryCreatedConsumer"/> class.
    /// </summary>
    /// <param name="idempotentMessageService">Service to manage idempotent messages.</param>
    /// <param name="obsoleteMessageService">Service to prevent obsoletes messages.</param>
    /// <param name="unitOfWork">Manage a <see cref="IDbSession"/> to encapsulate a business transaction which can affect the database.</param>
    /// <param name="projector">Manages proyection operations from "source" database to "projection" (read) database.</param>
    /// <exception cref="ArgumentNullException">When some argument for the constructor parameters is null.</exception>
    public CountryCreatedConsumer(
        IIdempotentMessageService idempotentMessageService,
        IObsoleteMessageService obsoleteMessageService,
        IUnitOfWork unitOfWork,
        ICountryCrearedProjector projector)
        : base(idempotentMessageService, obsoleteMessageService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _projector = projector ?? throw new ArgumentNullException(nameof(projector));
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Consumes the message.
    /// </summary>
    /// <param name="context">Context that allow access to details surrounding the inbound message and its metadata, including headers.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public override async Task Consume(ConsumeContext<Message<CountryCreatedIntegrationEvent>> context)
    {
        ArgumentNullException.ThrowIfNull(context);

        try
        {
            if (await NotYetConsumed(context.Message.Metadata))
            {
                Log.Information($"[CountryCreatedConsumer << CountryCreatedIntegrationEvent] Name => {context.Message.Content.Name}");

                // Projects the Create.
                _unitOfWork.BeginTransaction();
                await _projector.ProjectAsync(context.Message.Content.IataCode);
                _unitOfWork.Commit();
            }
        }
        catch
        {
            // TODO: Handle the exception (log, revert, etc.).
            throw;
        }
    }

    #endregion
}
