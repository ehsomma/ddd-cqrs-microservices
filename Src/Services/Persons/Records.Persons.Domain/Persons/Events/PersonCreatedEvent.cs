#region Usings

using Records.Shared.Domain.Events;
using DomainModel = Records.Persons.Domain.Persons.Models;

#endregion

namespace Records.Persons.Domain.Persons.Events;

/// <summary>
/// Represents an event to indicate that a <see cref="DomainModel.Person"/> was created.
/// </summary>
public sealed class PersonCreatedEvent : DomainEvent
{
    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="PersonCreatedEvent"/> class.
    /// </summary>
    /// <param name="createdPerson">The <see cref="DomainModel.Person"/> created.</param>
    public PersonCreatedEvent(DomainModel.Person createdPerson)
        : base(createdPerson.Id.ToString())
    {
        Person = createdPerson;
    }

    #endregion

    #region Properties

    /// <summary>The <see cref="DomainModel.Person"/> created.</summary>
    public DomainModel.Person Person { get; private set; }

    #endregion
}
