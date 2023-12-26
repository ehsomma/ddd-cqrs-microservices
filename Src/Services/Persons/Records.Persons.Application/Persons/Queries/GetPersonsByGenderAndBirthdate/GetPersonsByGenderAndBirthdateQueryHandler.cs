#region Usings

using Mapster;
using Records.Persons.Reads.Persons.GetPersonsByGenderAndBirthdate;
using Records.Shared.Application.Messaging;
using ReadModel = Records.Persons.Reads.Persons.Models; // Using aliases.
using ReadQuery = Records.Persons.Reads.Persons.GetPersonsByGenderAndBirthdate; // Using aliases.

#endregion

namespace Records.Persons.Application.Persons.Queries.GetPersonsByGenderAndBirthdate;

/// <summary>
/// Represents the <see cref="GetPersonsByGenderAndBirthdateQuery"/> handler.
/// </summary>
internal sealed class GetPersonsByGenderAndBirthdateQueryHandler : IQueryHandler<GetPersonsByGenderAndBirthdateQuery, IList<ReadModel.Person>?>
{
    #region Declarations

    private readonly IGetPersonsByGenderAndBirthdateRepository _repository;

    #endregion

    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="GetPersonsByGenderAndBirthdateQueryHandler"/> class.
    /// </summary>
    /// <param name="repository">Represents the operations to read from the Read (projection) database/repository.</param>
    /// <exception cref="ArgumentNullException">When some argument for the constructor parameters is null.</exception>
    public GetPersonsByGenderAndBirthdateQueryHandler(IGetPersonsByGenderAndBirthdateRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    #endregion

    #region Public methods

    /// <inheritdoc />
    public async Task<IList<ReadModel.Person>?> Handle(GetPersonsByGenderAndBirthdateQuery query, CancellationToken cancellationToken)
    {
        // Mapps the application query (with MediatR references) to read query (DTO).
        ReadQuery.GetPersonsByGenderAndBirthdateQuery readQuery =
            query.Adapt<ReadQuery.GetPersonsByGenderAndBirthdateQuery>();

        // NOTE: You can send primitives parameters to the _repository.GetAsync like in
        // GetPersonByIdQueryHandler instead.
        // We are sending an object just to demostrate that if you want to use an object because you
        // have a query with a lot properties, you must map it to a DTO.

        IList<ReadModel.Person>? persons = await _repository.GetAsync(readQuery, cancellationToken);

        return persons;
    }

    #endregion
}
