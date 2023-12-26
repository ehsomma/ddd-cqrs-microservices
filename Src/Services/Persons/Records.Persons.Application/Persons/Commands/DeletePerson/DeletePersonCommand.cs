using Records.Shared.Application.Messaging;

namespace Records.Persons.Application.Persons.Commands.DeletePerson;

/// <inheritdoc cref="ICommand"/>
public sealed class DeletePersonCommand : ICommand
{
    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="DeletePersonCommand"/> class.
    /// </summary>
    /// <param name="appKey">The appKey (just an example).</param>
    /// <param name="personId">The person ID to delete.</param>
    public DeletePersonCommand(string appKey, Guid personId)
    {
        AppKey = appKey;
        PersonId = personId;
    }

    #endregion

    #region Properties

    /// <summary>The person ID to delete.</summary>
    public Guid PersonId { get; init; }

    /// <summary>The appKey (just an example).</summary>
    public string AppKey { get; init; }

    #endregion
}
