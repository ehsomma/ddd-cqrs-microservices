#region Usings

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Records.Countries.Application.Countries.Commands.DeleteCountry;
using Records.Countries.Contracts.Countries;
using Records.Shared.Contracts;
using Records.Shared.Contracts.Abstractions.Mappers;

#endregion

namespace Records.Countries.Api.V1.Countries.Controllers.DeleteCountry;

/// <summary>
/// Controller for the endpoint.
/// </summary>
[ApiController]
[Tags("Countries")] // Controller name in Swagger.
[Produces("application/json")]
public class DeleteCountryController : ControllerBase
{
    #region Declarations

    /// <summary>MediatR library to send and handle commands and queries implementing CQRS.</summary>
    private readonly IMediator _mediator;

    /// <summary>Mapper to map the request to a command.</summary>
    private readonly IRequestToCommandMapper<DeleteCountryRequest, DeleteCountryCommand> _deleteCountryMapper;

    #endregion

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteCountryController"/> class.
    /// </summary>
    /// <param name="mediator">MediatR library to send and handle commands and queries implementing CQRS.</param>
    /// <param name="createCountryMapper">Mapper to map the request to a command.</param>
    /// <exception cref="ArgumentNullException">When some argument for the constructor parameters is null.</exception>
    public DeleteCountryController(
        IMediator mediator,
        IRequestToCommandMapper<DeleteCountryRequest, DeleteCountryCommand> createCountryMapper)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _deleteCountryMapper = createCountryMapper ?? throw new ArgumentNullException(nameof(createCountryMapper));
    }

    #endregion

    #region Endpoints

    /// <summary>
    /// Deletes the Country specified in the <paramref name="request"/>.
    /// </summary>
    /// <param name="request">The request to delete a Country.</param>
    /// <returns>NoContent if the operation was successful. Otherwise, an <see cref="ErrorResponse"/>.</returns>
    /// <response code="204">If the operation was successful.</response>
    /// <response code="500">If an unhandled error occurs.</response>
    [HttpDelete]
    [Route("countries")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteCountryAsync(DeleteCountryRequest request)
    {
        DeleteCountryCommand command = _deleteCountryMapper.FromRequestToCommand(request);

        await _mediator.Send(command);

        return NoContent();
    }

    #endregion
}
