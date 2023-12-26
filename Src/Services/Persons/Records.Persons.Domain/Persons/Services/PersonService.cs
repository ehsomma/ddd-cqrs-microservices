#region Usings

using Records.Persons.Domain.Persons.Errors;
using Records.Persons.Domain.Persons.Models;
using Records.Persons.Domain.Persons.Repositories;
using Records.Shared.Domain.Exceptions;

#endregion

namespace Records.Persons.Domain.Persons.Services;

// Services policies:
// • Contains domain operations with access to the repository.
// • Contains non basic operations that use other aggregates.
// • Receives the repository by constructor.
// • Can not use other repos.

/// <summary>
/// Represents the person service.
/// </summary>
public sealed class PersonService : IPersonService
{
    #region Declarations

    /// <summary>Represents the <see cref="Person"/> persistence operations.</summary>
    private readonly IPersonRepository _repository;

    #endregion

    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="PersonService"/> class.
    /// </summary>
    /// <param name="repository">Represents the &lt;see cref="Person"/&gt; persistence operations.</param>
    /// <exception cref="ArgumentNullException">When some argument for the constructor parameters is null.</exception>
    public PersonService(IPersonRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Gets the <see cref="Person"/> corresponding to the specified <paramref name="id"/>.
    /// </summary>
    /// <param name="id">The person ID to search for.</param>
    /// <returns>A <see cref="Person"/> or null.</returns>
    /// <exception cref="DomainException">When the <see cref="Person"/> is not found.</exception>
    public async Task<Person?> GetByIdAsync(Guid id)
    {
        Person? person = await _repository.GetByIdAsync(id);

        if (person == null)
        {
            throw new DomainException(DomainErrors.Person.NotFound);
        }

        return person;
    }

    #endregion
}
