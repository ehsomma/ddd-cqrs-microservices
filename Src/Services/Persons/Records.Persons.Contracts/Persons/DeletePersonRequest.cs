using Records.Shared.Contracts;

namespace Records.Persons.Contracts.Persons;

/// <summary>
/// Represents a request to delete a Person.
/// </summary>
public class DeletePersonRequest : Request
{
    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="DeletePersonRequest"/> class.
    /// </summary>
    /// <param name="appKey">The key to identify from what application the request is coming from.</param>
    /// <param name="personId">The id of the Person to delete.</param>
    public DeletePersonRequest(string appKey, Guid personId)
        : base(appKey)
    {
        PersonId = personId;
    }

    #endregion

    #region Properties

    /// <summary>The id of the Person to delete.</summary>
    public Guid PersonId { get; init; }

    #endregion
}
