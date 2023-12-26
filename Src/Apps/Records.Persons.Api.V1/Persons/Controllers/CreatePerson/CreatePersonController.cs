#region Usings

using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Records.Persons.Application.Persons.Commands.CreatePerson;
using Records.Persons.Contracts.Persons;
using Records.Shared.Contracts;
using Records.Shared.Contracts.Abstractions.Mappers;

#endregion

namespace Records.Persons.Api.V1.Persons.Controllers.CreatePerson;

/// <summary>
/// Controller for the endpoint.
/// </summary>
[ApiController]
[Tags("Persons")] // Controller name in Swagger.
[Produces("application/json")]
public class CreatePersonController : ControllerBase
{
    #region Declarations

    /// <summary>MediatR library to send and handle commands and queries implementing CQRS.</summary>
    private readonly IMediator _mediator;

    /// <summary>Mapper to map the request to a command.</summary>
    private readonly IRequestToCommandMapper<CreatePersonRequest, CreatePersonCommand> _createPersonMapper;

    /// <summary>The validator for the request.</summary>
    private readonly IValidator<CreatePersonRequest> _createPersonRequestValidator;

    #endregion

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CreatePersonController"/> class.
    /// </summary>
    /// <param name="mediator">MediatR library to send and handle commands and queries implementing CQRS.</param>
    /// <param name="createPersonMapper">Mapper to map the request to a command.</param>
    /// <param name="createPersonRequestValidator">The validator for the request.</param>
    /// <exception cref="ArgumentNullException">When some argument for the constructor parameters is null.</exception>
    public CreatePersonController(
        IMediator mediator,
        IRequestToCommandMapper<CreatePersonRequest, CreatePersonCommand> createPersonMapper,
        IValidator<CreatePersonRequest> createPersonRequestValidator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _createPersonMapper = createPersonMapper ?? throw new ArgumentNullException(nameof(createPersonMapper));
        _createPersonRequestValidator = createPersonRequestValidator ?? throw new ArgumentNullException(nameof(createPersonRequestValidator));
    }

    #endregion

    #region Endpoints

    /// <summary>
    /// Create a new Person with the specified data in the <paramref name="request"/>.
    /// </summary>
    /// <param name="request">The request to create a Person.</param>
    /// <returns>NoContent if the operation was successful. Otherwise, an <see cref="ErrorResponse"/>.</returns>
    /// <response code="204">If the operation was successful.</response>
    /// <response code="500">If an unhandled error occurs.</response>
    [HttpPost]
    [Route("persons")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PostPersonAsync(CreatePersonRequest request)
    {
        _createPersonRequestValidator.ValidateAndThrowExeption(request);

        CreatePersonCommand command = _createPersonMapper.FromRequestToCommand(request);

        await _mediator.Send(command);

        return NoContent();
    }

    #endregion
}
