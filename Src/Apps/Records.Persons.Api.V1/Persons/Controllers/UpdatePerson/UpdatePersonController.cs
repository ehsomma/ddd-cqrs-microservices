#region Usings

using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Records.Persons.Application.Persons.Commands.UpdatePerson;
using Records.Persons.Contracts.Persons;
using Records.Shared.Contracts;
using Records.Shared.Contracts.Abstractions.Mappers;

#endregion

namespace Records.Persons.Api.V1.Persons.Controllers.UpdatePerson;

/// <summary>
/// Controller for the endpoint.
/// </summary>
[ApiController]
[Tags("Persons")] // Controller name in Swagger.
public class UpdatePersonController : ControllerBase
{
    #region Declarations

    /// <summary>MediatR library to send and handle commands and queries implementing CQRS.</summary>
    private readonly IMediator _mediator;

    /// <summary>Mapper to map the request to a command.</summary>
    private readonly IRequestToCommandMapper<UpdatePersonRequest, UpdatePersonCommand> _updatePersonMapper;

    /// <summary>UpdatePersonRequest validator.</summary>
    private readonly IValidator<UpdatePersonRequest> _updatePersonRequestValidator;

    #endregion

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdatePersonController"/> class.
    /// </summary>
    /// <param name="mediator">MediatR library to send and handle commands and queries implementing CQRS.</param>
    /// <param name="updatePersonMapper">Mapper to map the request to a command.</param>
    /// <param name="updatePersonRequestValidator">The validator for the request.</param>
    /// <exception cref="ArgumentNullException">When some argument for the constructor parameters is null.</exception>
    public UpdatePersonController(
    IMediator mediator,
    IRequestToCommandMapper<UpdatePersonRequest, UpdatePersonCommand> updatePersonMapper,
    IValidator<UpdatePersonRequest> updatePersonRequestValidator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _updatePersonMapper = updatePersonMapper ?? throw new ArgumentNullException(nameof(updatePersonMapper));
        _updatePersonRequestValidator = updatePersonRequestValidator ?? throw new ArgumentNullException(nameof(updatePersonRequestValidator));
    }

    #endregion

    #region Endpoints

    /// <summary>
    /// Updates the specified Person with the specified data in the <paramref name="request"/>.
    /// </summary>
    /// <param name="request">The request to create a Person.</param>
    /// <returns>NoContent if the operation was successful. Otherwise, an <see cref="ErrorResponse"/>.</returns>
    /// <response code="204">If the operation was successful.</response>
    /// <response code="500">If an unhandled error occurs.</response>
    [HttpPut]
    [Route("persons")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PutPersonAsync(UpdatePersonRequest request)
    {
        ////_updatePersonRequestValidator.ValidateAndThrowExeption(request);

        UpdatePersonCommand command = _updatePersonMapper.FromRequestToCommand(request);

        await _mediator.Send(command);

        return NoContent();
    }

    #endregion
}
