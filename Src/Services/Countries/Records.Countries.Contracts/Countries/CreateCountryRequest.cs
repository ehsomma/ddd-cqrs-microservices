#region Usings

using Records.Shared.Contracts;
using Dto = Records.Countries.Dtos.Countries; // Using aliases.

#endregion

namespace Records.Countries.Contracts.Countries;

/// <summary>
/// Represents a request to create a Country.
/// </summary>
public class CreateCountryRequest : Request
{
    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateCountryRequest"/> class.
    /// </summary>
    /// <param name="appKey">The key to identify from what application the request is coming from.</param>
    /// <param name="country">The data to create the Country.</param>
    public CreateCountryRequest(string appKey, Dto.Country country)
        : base(appKey)
    {
        Country = country;
    }

    #endregion

    #region Properties

    /// <inheritdoc cref="Dto.Country"/>
    public Dto.Country Country { get; init; }

    #endregion
}
