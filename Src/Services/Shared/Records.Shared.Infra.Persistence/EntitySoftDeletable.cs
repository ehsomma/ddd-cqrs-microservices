using Records.Shared.Infra.Persistence.Abstractions;

namespace Records.Shared.Infra.Persistence;

/// <summary>
/// Represents a soft deletable entity.
/// </summary>
public class EntitySoftDeletable : IEntitySoftDeletable
{
    #region Public methods

    /// <inheritdoc />
    public DateTime? DeletedOnUtc { get; set; }

    /// <inheritdoc />
    public bool IsDeleted { get; set; }

    #endregion
}
