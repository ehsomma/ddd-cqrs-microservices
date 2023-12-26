#region Usings

using Records.Persons.Reads.Persons.GetPersonById;
using Records.Shared.Application.Messaging;
using ReadModel = Records.Persons.Reads.Persons.Models;

#endregion

namespace Records.Persons.Application.Persons.Queries.GetPersonById;

/// <summary>
/// Represents the <see cref="GetPersonByIdQuery"/> handler.
/// </summary>
internal sealed class GetPersonByIdQueryHandler : IQueryHandler<GetPersonByIdQuery, ReadModel.Person?>
{
    #region Declarations

    /// <summary>Represents the operations to read from the Read (projection) database/repository.</summary>
    private readonly IGetPersonByIdRepository _readRepository;

    #endregion

    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="GetPersonByIdQueryHandler"/> class.
    /// </summary>
    /// <param name="repository">Represents the operations to read from the Read (projection) database/repository.</param>
    /// <exception cref="ArgumentNullException">When some argument for the constructor parameters is null.</exception>
    public GetPersonByIdQueryHandler(IGetPersonByIdRepository repository)
    {
        _readRepository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    #endregion

    /// <inheritdoc />
    public async Task<ReadModel.Person?> Handle(GetPersonByIdQuery query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        ReadModel.Person? person = await _readRepository.GetAsync(query.Id, cancellationToken);

        return person;
    }
}
