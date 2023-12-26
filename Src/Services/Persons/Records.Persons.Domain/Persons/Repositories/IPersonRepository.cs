using Records.Persons.Domain.Persons.Models;

namespace Records.Persons.Domain.Persons.Repositories;

/// <summary>
/// Defines the repository for <see cref="Person"/>.
/// </summary>
public interface IPersonRepository
{
    #region Public methods

    /// <summary>
    /// Inserts the specified person into the repository.
    /// </summary>
    /// <param name="person">The Person to insert.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task InsertAsync(Person person);

    /// <summary>
    /// Update the specified person in the repository.
    /// </summary>
    /// <param name="person">The Person to update.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task UpdateAsync(Person person);

    /// <summary>
    /// Deletes the specified person from the repository.
    /// </summary>
    /// <param name="person">The Person to delete.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task DeleteAsync(Person person);

    /// <summary>
    /// Get the <see cref="Person"/> corresponding to the specified <paramref name="id"/>.
    /// </summary>
    /// <param name="id">The identification to search for.</param>
    /// <returns>The corresponding Person.</returns>
    public Task<Person?> GetByIdAsync(Guid id);

    #endregion
}
