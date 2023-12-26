#region Usings

using Microsoft.AspNetCore.Mvc;
using Records.Countries.Infra.Client;
using Records.Countries.Reads.Models.Countries;
using Records.Shared.Contracts;

#endregion

namespace Records.Persons.Api.V1.Countries.Controllers.GetCountriesSummary;

/// <summary>
/// Controller for the endpoint.
/// </summary>
[ApiController]
[Tags("Countries")] // Controller name in Swagger.
[Produces("application/json")]
public class GetCountriesSummaryController
{
    #region Declarations

    /// <summary>The typed httpClient for Countries microservice.</summary>
    private readonly CountriesClient _countriesClient;

    #endregion

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="GetCountriesSummaryController"/> class.
    /// </summary>
    /// <param name="countriesClient">The typed httpClient for Countries microservice.</param>
    /// <exception cref="ArgumentNullException">When some argument for the constructor parameters is null.</exception>
    public GetCountriesSummaryController(CountriesClient countriesClient)
    {
        _countriesClient = countriesClient ?? throw new ArgumentNullException(nameof(countriesClient));
    }

    #endregion

    #region Endpoints

    /// <summary>
    /// Gets a list of CountrySummary (from Countries microservice).
    /// </summary>
    /// <returns>A list of <see cref="CountrySummary"/>.</returns>
    /// <response code="200">If the operation was successful.</response>
    /// <response code="500">If an unhandled error occurs.</response>
    [HttpGet]
    [Route("countries/summary")]
    [ProducesResponseType(typeof(IList<CountrySummary>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IList<CountrySummary>?> GetCountriesSummaryAsync()
    {
        // NOTE: This is just an example of how to communicate via API to another microservice (in
        // this example Countries) using a typed httpClinet (CountriesClient) applying resilience
        // policies such as Retry and CircuitBreaker. This is just an example of using a typed httpClient,
        // for a correct implementation it should be used in the application.

        IList<CountrySummary>? result = await _countriesClient.GetCountriesSummary();

        /*
        IList<CountrySummary>? result;
        try
        {
            result = await _countriesClient.GetCountriesSummary();
        }
        catch (Polly.Timeout.TimeoutRejectedException ex)
        {
            throw;
        }
        catch (Polly.CircuitBreaker.BrokenCircuitException ex)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw;
        }
        */

        return result;
    }

    #endregion
}
