using Records.Persons.Domain.Persons.Models;
using Records.Shared.Domain.Exceptions;
using Records.Shared.Domain.Services;

namespace Records.Persons.Domain.Persons.Services;

/// <summary>
/// Defines the person service.
/// </summary>
public interface IPersonService : IDomainService
{
    /// <summary>
    /// Gets the <see cref="Person"/> corresponding to the specified <paramref name="id"/>.
    /// </summary>
    /// <param name="id">The person ID to search for.</param>
    /// <returns>A <see cref="Person"/> or null.</returns>
    /// <exception cref="DomainException">When the <see cref="Person"/> is not found.</exception>
    Task<Person?> GetByIdAsync(Guid id);
}
