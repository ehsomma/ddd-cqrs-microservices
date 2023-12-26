namespace Records.Shared.Domain.Models;

/// <summary>
/// Defines an auditable entity.
/// </summary>
public interface IEntityAuditable
{
    #region Properties

    /// <summary>
    /// Gets the date and time (UTC) the entity was created on.
    /// </summary>
    public DateTime? CreatedOnUtc { get; }

    /// <summary>
    /// Gets the date and time (UTC) the entity was modified on.
    /// </summary>
    public DateTime? UpdatedOnUtc { get; }

    #endregion
}
