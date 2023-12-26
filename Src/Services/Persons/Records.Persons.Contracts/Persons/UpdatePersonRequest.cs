﻿#region Usings

using Records.Shared.Contracts;
using Dto = Records.Persons.Dtos.Persons; // Using aliases.

#endregion

namespace Records.Persons.Contracts.Persons;

/// <summary>
/// Represents a request to update a Person.
/// </summary>
public class UpdatePersonRequest : Request
{
    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CreatePersonRequest"/> class.
    /// </summary>
    /// <param name="appKey">The key to identify from what application the request is coming from.</param>
    /// <param name="person">The data to update the Person.</param>
    public UpdatePersonRequest(string appKey, Dto.Person person)
        : base(appKey)
    {
        Person = person;
    }

    #endregion

    #region Properties

    /// <inheritdoc cref="Dto.Person"/>
    public Dto.Person Person { get; init; }

    #endregion
}
