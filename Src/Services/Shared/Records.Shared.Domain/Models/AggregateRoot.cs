#region Usings

using Records.Shared.Domain.Events;
using System.Diagnostics.CodeAnalysis;

#endregion

namespace Records.Shared.Domain.Models;

/// <summary>
/// Represents an aggregate root.
/// </summary>
/// <typeparam name="TId">The type of the identifier.</typeparam>
public abstract class AggregateRoot<TId> : Entity<TId>
{
    #region Declarations

    [SuppressMessage(
        "StyleCop.CSharp.MaintainabilityRules",
        "SA1401:Fields should be private",
        Justification = "I prefer to use it as field just in protected fields in base classes like Repository base class.")]
    //// ReSharper disable once InconsistentNaming
    private readonly List<IDomainEvent> _domainEvents = new ();

    #endregion

    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="AggregateRoot{TId}"/> class.
    /// </summary>
    /// <param name="id">The entity id.</param>
    protected AggregateRoot(TId id)
        : base(id)
    {
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Gets a <see cref="IReadOnlyCollection{IDomainEvent}"/> of <see cref="IDomainEvent"/> and cleans them from
    /// the entity since after this it is the responsibility of the one who did the pull.
    /// </summary>
    /// <returns>A <see cref="IReadOnlyCollection{IDomainEvent}"/> of <see cref="IDomainEvent"/>.</returns>
    public IReadOnlyCollection<IDomainEvent> PullDomainEvents()
    {
        IReadOnlyCollection<IDomainEvent> domainEvents = new List<IDomainEvent>(_domainEvents.AsReadOnly());
        _domainEvents.Clear();

        return domainEvents;
    }

    #endregion

    #region Private methods

    /// <summary>
    /// Registers the specified <see cref="IDomainEvent"/> to the entity.
    /// </summary>
    /// <param name="eventItem">The <see cref="IDomainEvent"/> to register.</param>
    protected void RegisterDomainEvent(IDomainEvent eventItem)
    {
        _domainEvents.Add(eventItem);
    }

    #endregion
}
