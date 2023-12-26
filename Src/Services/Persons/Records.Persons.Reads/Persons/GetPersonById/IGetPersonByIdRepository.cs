#region Usings

using Records.Shared.Reads.Abstractions;
using ReadModel = Records.Persons.Reads.Persons.Models;

#endregion

namespace Records.Persons.Reads.Persons.GetPersonById;

/// <inheritdoc/>
public interface IGetPersonByIdRepository : IReadRepository<Guid, ReadModel.Person?>
{
}
