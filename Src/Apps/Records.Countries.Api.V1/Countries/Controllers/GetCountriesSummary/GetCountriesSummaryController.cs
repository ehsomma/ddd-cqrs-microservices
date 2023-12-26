#region Usings

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Records.Countries.Reads.Models.Countries;
using Records.Shared.Contracts;

#endregion

namespace Records.Countries.Api.V1.Countries.Controllers.GetCountriesSummary;

/// <summary>
/// Controller for the endpoint.
/// </summary>
[ApiController]
[Tags("Countries")] // Controller name in Swagger.
[Produces("application/json")]
public class GetCountriesSummaryController : ControllerBase
{
    #region Declarations

    /// <summary>MediatR library to send and handle commands and queries implementing CQRS.</summary>
#pragma warning disable IDE0052 // Remove unread private members
    private readonly IMediator _mediator;
#pragma warning restore IDE0052 // Remove unread private members

    #endregion

    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="GetCountriesSummaryController"/> class.
    /// </summary>
    /// <param name="mediator">MediatR library to send and handle commands and queries implementing CQRS.</param>
    /// <exception cref="ArgumentNullException">When some argument for the constructor parameters is null.</exception>
    public GetCountriesSummaryController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    #endregion

    #region Endpoints

    /// <summary>
    /// Gets the full list of CountrySummary.
    /// </summary>
    /// <returns>A list of <see cref="CountrySummary"/>.</returns>
    /// <response code="200">If the operation was successful.</response>
    /// <response code="500">If an unhandled error occurs.</response>
    [HttpGet]
    [Route("countries/summary")]
    [ProducesResponseType(typeof(IList<CountrySummary>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IList<CountrySummary>> GetCountriesSummaryAsync()
    {
        // NOTE: The repository and projection of Countries are not implemented in Countries
        // microservice, therefore, we mock the CountriesSummary list.
        // The purpose of this endpoint is to demonstrate how it can be called from another
        // microservice (from Persons) through CountriesClient applying resilience policies like
        // Retry and CircuitBreaker. See: GetCountriesSummaryController in Records.Persons.Api.V1.
        return await MockCountriesSummary();
    }

    #endregion

    #region Private methods

    private static async Task<IList<CountrySummary>> MockCountriesSummary()
    {
        IList<CountrySummary> countriesSummary = new List<CountrySummary>()
        {
            new CountrySummary() { IataCode = "US", Name = "United States" },
            new CountrySummary() { IataCode = "AR", Name = "Argentina" },
            new CountrySummary() { IataCode = "ES", Name = "Spain" },
            new CountrySummary() { IataCode = "MX", Name = "Mexico" },
        };

        return await Task.FromResult(countriesSummary);
    }

    #endregion
}
