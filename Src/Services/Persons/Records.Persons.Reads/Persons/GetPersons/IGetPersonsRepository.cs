#region Usings

using Records.Shared.Reads.Abstractions;
using ReadModel = Records.Persons.Reads.Persons.Models;

#endregion

namespace Records.Persons.Reads.Persons.GetPersons;

/// <inheritdoc/>
public interface IGetPersonsRepository : IReadRepository<IList<ReadModel.Person>?>
{
}
