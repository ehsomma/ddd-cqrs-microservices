#region Usings

using Records.Shared.Domain.Events;
using DomainModel = Records.Persons.Domain.Persons.Models;

#endregion

namespace Records.Persons.Domain.Persons.Events;

/// <summary>
/// Represents an event to indicate that a <see cref="DomainModel.Person"/> was deleted.
/// </summary>
public sealed class PersonDeletedEvent : DomainEvent
{
    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="PersonDeletedEvent"/> class.
    /// </summary>
    /// <param name="deletedPerson">The <see cref="DomainModel.Person"/> deleted.</param>
    public PersonDeletedEvent(DomainModel.Person deletedPerson)
        : base(deletedPerson.Id.ToString())
    {
        Person = deletedPerson;
    }

    #endregion

    #region Properties

    /// <summary>The <see cref="DomainModel.Person"/> deleted.</summary>
    public DomainModel.Person Person { get; private set; }

    #endregion
}
