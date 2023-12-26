#region Usings

using Records.Persons.Reads.Persons.GetPersons;
using Records.Shared.Application.Messaging;
using ReadModel = Records.Persons.Reads.Persons.Models;

#endregion

namespace Records.Persons.Application.Persons.Queries.GetPersons;

/// <summary>
/// Represents the <see cref="GetPersonsQuery"/> handler.
/// </summary>
internal sealed class GetPersonsQueryHandler : IQueryHandler<GetPersonsQuery, IList<ReadModel.Person>?>
{
    #region Declarations

    /// <summary>Represents the operations to read from the Read (projection) database/repository.</summary>
    private readonly IGetPersonsRepository _repository;

    #endregion

    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="GetPersonsQueryHandler"/> class.
    /// </summary>
    /// <param name="repository">Represents the operations to read from the Read (projection) database/repository.</param>
    /// <exception cref="ArgumentNullException">When some argument for the constructor parameters is null.</exception>
    public GetPersonsQueryHandler(IGetPersonsRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    #endregion

    #region Public methods

    /// <inheritdoc />
    public async Task<IList<ReadModel.Person>?> Handle(GetPersonsQuery query, CancellationToken cancellationToken)
    {
        IList<ReadModel.Person>? persons = await _repository.GetAsync(cancellationToken);

        return persons;
    }

    #endregion
}
