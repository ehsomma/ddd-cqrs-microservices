#region Usings

using Records.Shared.Domain.Events;
using DomainModel = Records.Persons.Domain.Countries.Models;

#endregion

namespace Records.Persons.Domain.Countries.Events;

/// <summary>
/// Represents an event to indicate that a <see cref="DomainModel.Country"/> was deleted.
/// </summary>
public sealed class CountryDeletedEvent : DomainEvent
{
    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CountryDeletedEvent"/> class.
    /// </summary>
    /// <param name="deletedContry">The deleted <see cref="Country"/>.</param>
    public CountryDeletedEvent(DomainModel.Country deletedContry)
        : base(deletedContry.IataCode.ToString())
    {
        Country = deletedContry;
    }

    #endregion

    #region Public methods

    /// <summary>The deleted <see cref="Country"/>.</summary>
    public DomainModel.Country Country { get; }

    #endregion
}
