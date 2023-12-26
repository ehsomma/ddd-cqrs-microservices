using Records.Shared.Projection.Abstractions;

namespace Records.Persons.Projection.Persons.Projectors;

/// <summary>
/// Defines the PersonUpdatedProjector. Projectors project (copy) data from "source" database to
/// "projection" (read) database.
/// </summary>
public interface IPersonUpdatedProjector : IProjector
{
    #region Public methods

    /// <summary>
    /// Projects the update of the Person corresponding to the specified ID.
    /// </summary>
    /// <param name="id">The ID of the person to project the update.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task ProjectAsync(Guid id);

    #endregion
}
