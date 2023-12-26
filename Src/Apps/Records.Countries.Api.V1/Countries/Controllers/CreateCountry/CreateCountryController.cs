#region Usings

using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Records.Countries.Application.Countries.Commands.CreateCountry;
using Records.Countries.Contracts.Countries;
using Records.Shared.Contracts;
using Records.Shared.Contracts.Abstractions.Mappers;

#endregion

namespace Records.Countries.Api.V1.Countries.Controllers.CreateCountry;

/// <summary>
/// Controller for the endpoint.
/// </summary>
[ApiController]
[Tags("Countries")] // Controller name in Swagger.
[Produces("application/json")]
public class CreateCountryController : ControllerBase
{
    #region Declarations

    /// <summary>MediatR library to send and handle commands and queries implementing CQRS.</summary>
    private readonly IMediator _mediator;

    /// <summary>Mapper to map the request to a command.</summary>
    private readonly IRequestToCommandMapper<CreateCountryRequest, CreateCountryCommand> _createCountryMapper;

    /// <summary>CreatePersonRequest validator.</summary>
    private readonly IValidator<CreateCountryRequest> _createCountryRequestValidator;

    #endregion

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateCountryController"/> class.
    /// </summary>
    /// <param name="mediator">MediatR library to send and handle commands and queries implementing CQRS.</param>
    /// <param name="createCountryMapper">Mapper to map the request to a command.</param>
    /// <param name="createCountryRequestValidator">The validator for the request.</param>
    /// <exception cref="ArgumentNullException">When some argument for the constructor parameters is null.</exception>
    public CreateCountryController(
        IMediator mediator,
        IRequestToCommandMapper<CreateCountryRequest, CreateCountryCommand> createCountryMapper,
        IValidator<CreateCountryRequest> createCountryRequestValidator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _createCountryMapper = createCountryMapper ?? throw new ArgumentNullException(nameof(createCountryMapper));
        _createCountryRequestValidator = createCountryRequestValidator ?? throw new ArgumentNullException(nameof(createCountryRequestValidator));
    }

    #endregion

    #region Endpoints

    /// <summary>
    /// Creates a new Country.
    /// </summary>
    /// <param name="request">The request to create a Country.</param>
    /// <returns>NoContent if the operation was successful. Otherwise, an <see cref="ErrorResponse"/>.</returns>
    /// <remarks>
    /// Some remark text.
    /// </remarks>
    /// <response code="204">If the operation was successful.</response>
    /// <response code="500">If an unhandled error occurs.</response>
    [HttpPost]
    [Route("countries")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PostCountryAsync(CreateCountryRequest request)
    {
        _createCountryRequestValidator.ValidateAndThrowExeption(request);

        CreateCountryCommand command = _createCountryMapper.FromRequestToCommand(request);

        await _mediator.Send(command);

        return NoContent();
    }

    #endregion
}
