namespace Records.Shared.Infra.Persistence.Abstractions;

/// <summary>
/// Represents the marker interface for auditable entities.
/// </summary>
public interface IEntityAuditable
{
    #region Properties

    /// <summary>
    /// Gets the date and time (UTC) the entity was created on.
    /// </summary>
    DateTime? CreatedOnUtc { get; init; }

    /// <summary>
    /// Gets the date and time (UTC) the entity was modified on.
    /// </summary>
    DateTime? UpdatedOnUtc { get; init; }

    #endregion
}
