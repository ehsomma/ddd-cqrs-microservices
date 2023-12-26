#region Usings

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Records.Persons.Application.Persons.Queries.GetPersonById;
using Records.Persons.Reads.Persons.Models;
using Records.Shared.Contracts;

#endregion

namespace Records.Persons.Api.V1.Persons.Controllers.GetPersonById;

/// <summary>
/// Controller for the endpoint.
/// </summary>
[ApiController]
[Tags("Persons")] // Controller name in Swagger.
public class GetPersonByIdController : ControllerBase
{
    #region Declarations

    /// <summary>MediatR library to send and handle commands and queries implementing CQRS.</summary>
    private readonly IMediator _mediator;

    #endregion

    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="GetPersonByIdController"/> class.
    /// </summary>
    /// <param name="mediator">MediatR library to send and handle commands and queries implementing CQRS.</param>
    /// <exception cref="ArgumentNullException">When some argument for the constructor parameters is null.</exception>
    public GetPersonByIdController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    #endregion

    #region Endpoints

    /// <summary>
    /// Gets the Person corresponding to the specified <paramref name="id"/>.
    /// </summary>
    /// <param name="id">The id to search.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The <see cref="Person"/> searched.</returns>
    /// <response code="200">If the operation was successful.</response>
    /// <response code="500">If an unhandled error occurs.</response>
    [HttpGet]
    [Route("personById")]
    [ProducesResponseType(typeof(Person), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<Person> GetPersonByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        GetPersonByIdQuery query = new (id);
        Person result = await _mediator.Send(query, cancellationToken);

        return result;
    }

    #endregion
}
