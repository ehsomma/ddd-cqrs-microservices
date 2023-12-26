#region Usings

using Records.Shared.Domain.Events;
using DomainModel = Records.Countries.Domain.Countries.Models;

#endregion

namespace Records.Countries.Domain.Countries.Events;

/// <summary>
/// Represents an event to indicate that a <see cref="Country"/> was created.
/// </summary>
public sealed class CountryCreatedEvent : DomainEvent
{
    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CountryCreatedEvent"/> class.
    /// </summary>
    /// <param name="createdCountry">The <see cref="DomainModel.Country"/> created.</param>
    public CountryCreatedEvent(DomainModel.Country createdCountry)
        : base(createdCountry.Id.ToString())
    {
        Country = createdCountry;
    }

    #endregion

    #region Properties

    /// <summary>The <see cref="DomainModel.Country"/> created.</summary>
    public DomainModel.Country Country { get; private set; }

    #endregion
}
