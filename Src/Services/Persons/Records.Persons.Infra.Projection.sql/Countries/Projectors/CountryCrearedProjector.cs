#region Usings

using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Configuration;
using Records.Persons.Projection.Countries.Projectors;
using Records.Shared.Projection.Abstractions;
////using Records.Shared.Infra.Persistence.Abstractions;
////using ReadModel = Records.Persons.Reads.Countries.Models;
using ProjectionDataModel = Records.Persons.Infra.Projection.sql.Countries.Models;

#endregion

namespace Records.Persons.Infra.Projection.sql.Countries.Projectors;

/// <inheritdoc cref="ICountryCrearedProjector"/>
public class CountryCrearedProjector : CountryProjector, ICountryCrearedProjector
{
    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CountryCrearedProjector"/> class.
    /// </summary>
    /// <param name="configuration">Represents a set of key/value application configuration properties.</param>
    /// <param name="dbSession">Represents a session in the database/UOW that contains a connection and a transaction.</param>
    public CountryCrearedProjector(
        IConfiguration configuration,
        IDbSession dbSession)
        : base(configuration, dbSession)
    {
    }

    #endregion

    #region Public methods

    /// <inheritdoc />
    public async Task ProjectAsync(string iataCode)
    {
        // Read ProjectionDataModel.Country from source repository.
        ProjectionDataModel.Country? country = await GetCountryAsync(iataCode);

        if (country != null)
        {
            // Write (project) ProjectionDataModel.Country into projection repository.
            await _dbSession.Connection.InsertAsync(country, _dbSession.Transaction);
        }
    }

    #endregion
}
