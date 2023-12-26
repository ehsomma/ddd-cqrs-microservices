#region Usings

using Mapster;
using Records.Persons.Reads.Persons.GetPersonsWithSpecificProfile;
using Records.Shared.Application.Messaging;
using ReadModel = Records.Persons.Reads.Persons.Models; // Using aliases.
using ReadQuery = Records.Persons.Reads.Persons.GetPersonsWithSpecificProfile; // Using aliases.

#endregion

namespace Records.Persons.Application.Persons.Queries.GetPersonsWithSpecificProfile;

/// <summary>
/// Represents the <see cref="GetPersonsWithSpecificProfileQuery"/> handler.
/// </summary>
internal sealed class GetPersonsWithSpecificProfileQueryHandler : IQueryHandler<GetPersonsWithSpecificProfileQuery, IList<ReadModel.Person>?>
{
    private readonly IGetPersonsWithSpecificProfileRepository _repository;

    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="GetPersonsWithSpecificProfileQueryHandler"/> class.
    /// </summary>
    /// <param name="repository">Represents the operations to read from the Read (projection) database/repository.</param>
    /// <exception cref="ArgumentNullException">When some argument for the constructor parameters is null.</exception>
    public GetPersonsWithSpecificProfileQueryHandler(IGetPersonsWithSpecificProfileRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    #endregion

    /// <inheritdoc />
    public async Task<IList<ReadModel.Person>?> Handle(GetPersonsWithSpecificProfileQuery query, CancellationToken cancellationToken)
    {
        // Mapps the application query (with MediatR references) to read query (DTO).
        ReadQuery.GetPersonsWithSpecificProfileQuery readQuery =
            query.Adapt<ReadQuery.GetPersonsWithSpecificProfileQuery>();

        // NOTE: You can send primitives parameters to the _repository.GetAsync like in
        // GetPersonByIdQueryHandler instead.
        // We are sending an object just to demostrate that if you want to use an object because you
        // have a query with a lot properties, you must map it to a DTO.

        IList<ReadModel.Person>? persons = await _repository.GetAsync(readQuery, cancellationToken);

        return persons;
    }
}
