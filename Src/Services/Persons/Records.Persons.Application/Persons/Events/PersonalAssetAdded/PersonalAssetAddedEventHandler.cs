#region Usings

using Records.Persons.Domain.Persons.Events;
using Records.Shared.Application.Messaging;
using Serilog;

#endregion

namespace Records.Persons.Application.Persons.Events.PersonalAssetAdded;

/// <summary>
/// Represents the <see cref="DomainMessage{PersonalAssetAddedEvent}"/> handler.
/// (TContent: <see cref="PersonalAssetAddedEvent"/>).
/// </summary>
internal sealed class PersonalAssetAddedEventHandler : IDomainEventHandler<DomainMessage<PersonalAssetAddedEvent>>
{
    #region Public methods

    /// <inheritdoc />
    public Task Handle(DomainMessage<PersonalAssetAddedEvent> message, CancellationToken cancellationToken)
    {
        Log.Information("[PersonalAssetAddedEventHandler << PersonalAssetAddedEvent] Handled");
        return Task.CompletedTask;
    }

    #endregion
}
