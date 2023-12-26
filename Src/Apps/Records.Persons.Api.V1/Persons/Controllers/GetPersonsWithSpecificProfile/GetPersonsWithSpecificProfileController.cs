#region Usings

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Records.Persons.Application.Persons.Queries.GetPersonsWithSpecificProfile;
using Records.Persons.Reads.Persons.Models;
using Records.Shared.Contracts;

#endregion

namespace Records.Persons.Api.V1.Persons.Controllers.GetPersonsWithSpecificProfile;

/// <summary>
/// Controller for the endpoint.
/// </summary>
[ApiController]
[Tags("Persons")] // Controller name in Swagger.
public class GetPersonsWithSpecificProfileController : ControllerBase
{
    #region Declarations

    /// <summary>MediatR library to send and handle commands and queries implementing CQRS.</summary>
    private readonly IMediator _mediator;

    #endregion

    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="GetPersonsWithSpecificProfileController"/> class.
    /// </summary>
    /// <param name="mediator">MediatR library to send and handle commands and queries implementing CQRS.</param>
    /// <exception cref="ArgumentNullException">When some argument for the constructor parameters is null.</exception>
    public GetPersonsWithSpecificProfileController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    #endregion

    #region Endpoints

    /// <summary>
    /// Gets the a list of Persons correspondind to the specified <paramref name="query"/>.
    /// </summary>
    /// <param name="query">The query typed filter.</param>
    /// <returns>A list of filtered Persons.</returns>
    /// <response code="200">If the operation was successful.</response>
    /// <response code="500">If an unhandled error occurs.</response>
    [HttpPost]
    [Route("personsWithSpecificProfile")]
    [ProducesResponseType(typeof(IList<Person>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IList<Person>> GetPersonsWithSpecificProfile([FromBody] GetPersonsWithSpecificProfileQuery query)
    {
        ////CreatePersonCommand command = _createPersonMapper.FromRequestToCommand(createPersonRequest);

        ////GetPersonsWithSpecificProfileQuery query = new();
        IList<Person> result = await _mediator.Send(query);

        return result;
    }

    #endregion
}
