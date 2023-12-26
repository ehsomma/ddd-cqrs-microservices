#region Usings

using Records.Shared.Application.Messaging;
using Dto = Records.Persons.Dtos.Persons; // Using aliases.

#endregion

namespace Records.Persons.Application.Persons.Commands.CreatePerson;

/// <inheritdoc cref="ICommand"/>
public sealed class CreatePersonCommand : ICommand
{
    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CreatePersonCommand"/> class.
    /// </summary>
    /// <param name="appKey">The appKey (just an example).</param>
    /// <param name="person">The <see cref="Dto.Person"/> object as content data.</param>
    public CreatePersonCommand(string appKey, Dto.Person person)
    {
        AppKey = appKey;
        Person = person;
    }

    #endregion

    #region Properties

    /// <summary>The <see cref="Dto.Person"/> object as content data.</summary>
    public Dto.Person Person { get; init; }

    /// <summary>The appKey (just an example).</summary>
    public string AppKey { get; init; }

    #endregion
}
