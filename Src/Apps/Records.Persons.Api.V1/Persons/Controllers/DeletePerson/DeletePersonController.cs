#region Usings

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Records.Persons.Application.Persons.Commands.DeletePerson;
using Records.Persons.Contracts.Persons;
using Records.Shared.Contracts;
using Records.Shared.Contracts.Abstractions.Mappers;

#endregion

namespace Records.Persons.Api.V1.Persons.Controllers.DeletePerson;

/// <summary>
/// Controller for the endpoint.
/// </summary>
[ApiController]
[Tags("Persons")] // Controller name in Swagger.
public class DeletePersonController : ControllerBase
{
    #region Declarations

    /// <summary>MediatR library to send and handle commands and queries implementing CQRS.</summary>
    private readonly IMediator _mediator;

    /// <summary>Mapper to map the request to a command.</summary>
    private readonly IRequestToCommandMapper<DeletePersonRequest, DeletePersonCommand> _deletePersonMapper;

    #endregion

    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="DeletePersonController"/> class.
    /// </summary>
    /// <param name="mediator">MediatR library to send and handle commands and queries implementing CQRS.</param>
    /// <param name="deletePersonMapper">Mapper to map the request to a command.</param>
    /// <exception cref="ArgumentNullException">When some argument for the constructor parameters is null.</exception>
    public DeletePersonController(
        IMediator mediator,
        IRequestToCommandMapper<DeletePersonRequest, DeletePersonCommand> deletePersonMapper)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _deletePersonMapper = deletePersonMapper ?? throw new ArgumentNullException(nameof(deletePersonMapper));
    }

    #endregion

    #region Endpoints

    /// <summary>
    /// Deletes the Person specified in the <paramref name="request"/>.
    /// </summary>
    /// <param name="request">The request to delete a Person.</param>
    /// <returns>NoContent if the operation was successful. Otherwise, an <see cref="ErrorResponse"/>.</returns>
    /// <response code="204">If the operation was successful.</response>
    /// <response code="500">If an unhandled error occurs.</response>
    [HttpDelete]
    [Route("persons")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeletePersonAsync(DeletePersonRequest request)
    {
        DeletePersonCommand command = _deletePersonMapper.FromRequestToCommand(request);

        await _mediator.Send(command);

        return NoContent();
    }

    #endregion
}
