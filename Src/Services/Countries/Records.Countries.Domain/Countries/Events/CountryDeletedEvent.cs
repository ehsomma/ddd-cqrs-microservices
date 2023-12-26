#region Usings

using Records.Shared.Domain.Events;
using DomainModel = Records.Countries.Domain.Countries.Models;

#endregion

namespace Records.Countries.Domain.Countries.Events;

/// <summary>
/// Represents an event to indicate that a <see cref="Country"/> was deleted.
/// </summary>
public sealed class CountryDeletedEvent : DomainEvent
{
    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CountryDeletedEvent"/> class.
    /// </summary>
    /// <param name="deletedCountry">The deleted <see cref="Country"/>.</param>
    public CountryDeletedEvent(DomainModel.Country deletedCountry)
        : base(deletedCountry.IataCode.ToString())
    {
        Country = deletedCountry;
    }

    #endregion

    #region Properties

    /// <summary>The deleted <see cref="Country"/>.</summary>
    public DomainModel.Country Country { get; }

    #endregion
}
