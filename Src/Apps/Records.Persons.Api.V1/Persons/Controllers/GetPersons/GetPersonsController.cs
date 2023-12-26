#region Usings

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Records.Persons.Application.Persons.Queries.GetPersons;
using Records.Persons.Reads.Persons.Models;

#endregion

namespace Records.Persons.Api.V1.Persons.Controllers.GetPersons;

/// <summary>
/// Controller for the endpoint.
/// </summary>
[ApiController]
[Tags("Persons")] // Controller name in Swagger.
[Produces("application/json")]
public class GetPersonsController : ControllerBase
{
    #region Declarations

    /// <summary>MediatR library to send and handle commands and queries implementing CQRS.</summary>
    private readonly IMediator _mediator;

    #endregion

    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="GetPersonsController"/> class.
    /// </summary>
    /// <param name="mediator">MediatR library to send and handle commands and queries implementing CQRS.</param>
    /// <exception cref="ArgumentNullException">When some argument for the constructor parameters is null.</exception>
    public GetPersonsController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    #endregion

    #region Endpoints

    /// <summary>
    /// Gets the full list of Persons.
    /// </summary>
    /// <returns>A list of Person.</returns>
    [HttpGet]
    [Route("persons")]
    public async Task<IList<Person>> GetPersonsAsync()
    {
        // TODO: Implement a paged list.

        GetPersonsQuery query = new ();
        IList<Person> result = await _mediator.Send(query);

        return result;
    }

    #endregion
}
