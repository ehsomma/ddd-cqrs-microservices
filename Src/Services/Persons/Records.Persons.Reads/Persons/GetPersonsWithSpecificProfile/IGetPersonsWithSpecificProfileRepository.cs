#region Usings

using Records.Shared.Reads.Abstractions;
using ReadModel = Records.Persons.Reads.Persons.Models;

#endregion

namespace Records.Persons.Reads.Persons.GetPersonsWithSpecificProfile;

/// <inheritdoc />
public interface IGetPersonsWithSpecificProfileRepository : IReadRepository<GetPersonsWithSpecificProfileQuery, IList<ReadModel.Person>?>
{
}
