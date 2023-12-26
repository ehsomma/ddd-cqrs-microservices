using Records.Shared.Contracts;

namespace Records.Countries.Contracts.Countries;

/// <summary>
/// Represents a request to delete a Country.
/// </summary>
public class DeleteCountryRequest : Request
{
    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteCountryRequest"/> class.
    /// </summary>
    /// <param name="appKey">The key to identify from what application the request is coming from.</param>
    /// <param name="iataCode">The IATA code of the country to delete.</param>
    public DeleteCountryRequest(string appKey, string iataCode)
        : base(appKey)
    {
        IataCode = iataCode;
    }

    #endregion

    #region Properties

    /// <summary>The IATA code of the country to delete for.</summary>
    public string IataCode { get; init; }

    #endregion
}
