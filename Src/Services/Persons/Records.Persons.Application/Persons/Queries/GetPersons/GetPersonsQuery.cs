#region Usings

using Records.Shared.Application.Messaging;
using ReadModel = Records.Persons.Reads.Persons.Models;

#endregion

namespace Records.Persons.Application.Persons.Queries.GetPersons;

/// <summary>
/// Represents a query to get all the Persons.
/// </summary>
public sealed class GetPersonsQuery : IQuery<IList<ReadModel.Person>>
{
}
