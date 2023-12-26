using Records.Shared.Infra.Persistence.Abstractions;

namespace Records.Shared.Infra.Persistence;

/// <summary>
/// Represents an auditable and soft deleteable entity.
/// </summary>
public class EntityAuditableSoftDeletable : IEntityAuditable, IEntitySoftDeletable
{
    #region Public methods

    /// <inheritdoc />
    public DateTime? CreatedOnUtc { get; init; }

    /// <inheritdoc />
    public DateTime? UpdatedOnUtc { get; init; }

    /// <inheritdoc />
    public DateTime? DeletedOnUtc { get; set; }

    /// <inheritdoc />
    public bool IsDeleted { get; set; }

    #endregion
}
