namespace Records.Shared.Infra.Persistence.Abstractions;

/// <summary>
/// Represents the marker interface for soft-deletable entities.
/// </summary>
public interface IEntitySoftDeletable
{
    #region Properties

    /// <summary>
    /// Gets the date and time (UTC) the entity was deleted on.
    /// </summary>
    DateTime? DeletedOnUtc { get; set; }

    /// <summary>
    /// Gets a value indicating whether the entity has been deleted.
    /// </summary>
    bool IsDeleted { get; set; }

    #endregion
}
