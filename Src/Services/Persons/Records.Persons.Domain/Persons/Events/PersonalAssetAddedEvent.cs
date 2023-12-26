#region Usings

using Records.Shared.Domain.Events;
using DomainModel = Records.Persons.Domain.Persons.Models;

#endregion

namespace Records.Persons.Domain.Persons.Events;

/// <summary>
/// Represents an event to indicate that a <see cref="DomainModel.PersonalAsset"/> was added.
/// </summary>
public sealed class PersonalAssetAddedEvent : DomainEvent
{
    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="PersonalAssetAddedEvent"/> class.
    /// </summary>
    /// <param name="person">The <see cref="DomainModel.Person"/> that the <see cref="DomainModel.PersonalAsset"/> belongs.</param>
    /// <param name="newPersonalAsset">The added <see cref="DomainModel.PersonalAsset"/>.</param>
    public PersonalAssetAddedEvent(DomainModel.Person person, DomainModel.PersonalAsset newPersonalAsset)
        : base(newPersonalAsset.Id.ToString())
    {
        Person = person;
        PersonalAsset = newPersonalAsset;
    }

    #endregion

    #region Properties

    /// <summary>The <see cref="DomainModel.Person"/> that the <see cref="DomainModel.PersonalAsset"/> belongs.</summary>
    public DomainModel.Person Person { get; private set; }

    /// <summary>The added <see cref="DomainModel.PersonalAsset"/>.</summary>
    public DomainModel.PersonalAsset PersonalAsset { get; private set; }

    #endregion
}
