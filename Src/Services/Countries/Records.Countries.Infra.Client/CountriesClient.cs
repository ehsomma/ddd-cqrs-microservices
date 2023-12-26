#region Usings

using Records.Countries.Reads.Models.Countries;
using System.Net.Http.Json;

#endregion

namespace Records.Countries.Infra.Client;

/// <summary>
/// Represents the typed httpClient for Countries microservice.
/// </summary>
public class CountriesClient
{
    #region Declarations

    /// <summary>Provides a class for sending HTTP requests and receiving HTTP responses from a resource identified by a URI.</summary>
    private readonly HttpClient _httpClient;

    #endregion

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CountriesClient"/> class.
    /// </summary>
    /// <param name="httpClient">Provides a class for sending HTTP requests and receiving HTTP responses from a resource identified by a URI.</param>
    public CountriesClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Gets a list of <see cref="CountrySummary"/> from the Countries microservice vía API.
    /// </summary>
    /// <returns>A list of <see cref="CountrySummary"/>.</returns>
    public async Task<IList<CountrySummary>?> GetCountriesSummary()
    {
        IList<CountrySummary>? content = await _httpClient.GetFromJsonAsync<List<CountrySummary>?>($"countries/summary");

        return content;
    }

    #endregion
}
