#region Usings

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Records.Persons.Application.Persons.Queries.GetPersonsByGenderAndBirthdate;
using Records.Persons.Reads.Persons.Models;
using Records.Shared.Contracts;

#endregion

namespace Records.Persons.Api.V1.Persons.Controllers.GetPersonsByGenderAndBirthday;

/// <summary>
/// Controller for the endpoint.
/// </summary>
[ApiController]
[Tags("Persons")] // Controller name in Swagger.
public class GetPersonsByGenderAndBirthdayController : ControllerBase
{
    #region Declarations

    /// <summary>MediatR library to send and handle commands and queries implementing CQRS.</summary>
    private readonly IMediator _mediator;

    #endregion

    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="GetPersonsByGenderAndBirthdayController"/> class.
    /// </summary>
    /// <param name="mediator">MediatR library to send and handle commands and queries implementing CQRS.</param>
    /// <exception cref="ArgumentNullException">When some argument for the constructor parameters is null.</exception>
    public GetPersonsByGenderAndBirthdayController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    #endregion

    #region Endpoints

    /// <summary>
    /// Gets the a list of Persons correspondind to the filter arguments.
    /// </summary>
    /// <param name="gender">The gender filter.</param>
    /// <param name="birthDateFrom">The birthdate from filter.</param>
    /// <param name="birthDateTo">The birthdate to filter.</param>
    /// <returns>A list of filtered Persons.</returns>
    /// <response code="200">If the operation was successful.</response>
    /// <response code="500">If an unhandled error occurs.</response>
    [HttpGet]
    [Route("personsByGenderAndBirthday")]
    [ProducesResponseType(typeof(IList<Person>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IList<Person>> GetPersonsByGenderAndBirthdayAsync(
        [FromQuery] string gender,
        [FromQuery] DateTime birthDateFrom,
        [FromQuery] DateTime birthDateTo)
    {
        GetPersonsByGenderAndBirthdateQuery query = new (gender, birthDateFrom, birthDateTo);
        IList<Person> result = await _mediator.Send(query);

        return result;
    }

    #endregion
}
