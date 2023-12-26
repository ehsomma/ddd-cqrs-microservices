#region Usings

using Records.Shared.Domain.Events;
using DomainModel = Records.Persons.Domain.Countries.Models;

#endregion

namespace Records.Persons.Domain.Countries.Events;

/// <summary>
/// Represents an event to indicate that a <see cref="DomainModel.Country"/> was created.
/// </summary>
public sealed class CountryCreatedEvent : DomainEvent
{
    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CountryCreatedEvent"/> class.
    /// </summary>
    /// <param name="createdCountry">The <see cref="DomainModel.Country"/> created.</param>
    public CountryCreatedEvent(DomainModel.Country createdCountry)
        : base(createdCountry.IataCode)
    {
        Country = createdCountry;
    }

    #endregion

    #region Properties

    /// <summary>The <see cref="Country"/> created.</summary>
    public DomainModel.Country Country { get; private set; }

    #endregion
}
