using Records.Shared.Projection.Abstractions;

namespace Records.Persons.Projection.Persons.Projectors;

/// <summary>
/// Defines the PersonCreatedProjector. Projectors project (copy) data from "source" database to
/// "projection" (read) database.
/// </summary>
public interface IPersonCreatedProjector : IProjector
{
    #region Public methods

    /// <summary>
    /// Projects the Person corresponding to the specified person Id code.
    /// </summary>
    /// <param name="id">The ID of the person to project.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task ProjectAsync(Guid id);

    #endregion
}
