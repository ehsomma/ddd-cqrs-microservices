#region Usings

using ReadModel = Records.Persons.Reads.Persons.Models;

#endregion

namespace Records.Persons.Reads.Persons.GetPersonById;

/// <summary>
/// Represents a query to get a Person corresponding to the specified ID.
/// </summary>
public sealed class GetPersonByIdQuery
{
    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="GetPersonByIdQuery"/> class.
    /// </summary>
    /// <param name="id">The ID of the <see cref="ReadModel.Person"/> to get.</param>
    public GetPersonByIdQuery(Guid id)
    {
        Id = id;
    }

    #endregion

    #region Properties

    /// <summary>The ID of the <see cref="ReadModel.Person"/> to get.</summary>
    public Guid Id { get; } // Readonly.

    #endregion
}
