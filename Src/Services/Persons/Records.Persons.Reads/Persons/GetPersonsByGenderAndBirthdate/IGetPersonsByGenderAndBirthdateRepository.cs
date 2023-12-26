#region Usings

using Records.Shared.Reads.Abstractions;
using ReadModel = Records.Persons.Reads.Persons.Models;

#endregion

namespace Records.Persons.Reads.Persons.GetPersonsByGenderAndBirthdate;

/// <inheritdoc />
public interface IGetPersonsByGenderAndBirthdateRepository : IReadRepository<GetPersonsByGenderAndBirthdateQuery, IList<ReadModel.Person>?>
{
}
