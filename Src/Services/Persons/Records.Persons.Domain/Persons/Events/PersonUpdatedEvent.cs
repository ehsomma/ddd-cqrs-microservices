#region Usings

using Records.Shared.Domain.Events;
using DomainModel = Records.Persons.Domain.Persons.Models;

#endregion

namespace Records.Persons.Domain.Persons.Events;

/// <summary>
/// Represents an event to indicate that a <see cref="DomainModel.Person"/> was updated.
/// </summary>
public sealed class PersonUpdatedEvent : DomainEvent
{
    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="PersonUpdatedEvent"/> class.
    /// </summary>
    /// <param name="updatedPerson">The updated <see cref="DomainModel.Person"/>.</param>
    public PersonUpdatedEvent(DomainModel.Person updatedPerson)
        : base(updatedPerson.Id.ToString())
    {
        Person = updatedPerson;
    }

    #endregion

    #region Properties

    /// <summary>The updated <see cref="DomainModel.Person"/>.</summary>
    public DomainModel.Person Person { get; private set; }

    #endregion
}
