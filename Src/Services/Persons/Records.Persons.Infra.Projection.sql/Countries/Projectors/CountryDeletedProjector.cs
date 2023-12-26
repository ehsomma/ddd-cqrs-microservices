#region Usings

using Dapper;
using Microsoft.Extensions.Configuration;
using Records.Persons.Projection.Countries.Projectors;
using Records.Shared.Projection.Abstractions;
////using Records.Shared.Infra.Persistence.Abstractions;
////using ReadModel = Records.Persons.Reads.Persons.Models;

#endregion

namespace Records.Persons.Infra.Projection.sql.Countries.Projectors;

/// <inheritdoc cref="ICountryDeletedProjector"/>
public class CountryDeletedProjector : CountryProjector, ICountryDeletedProjector
{
    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CountryDeletedProjector"/> class.
    /// </summary>
    /// <param name="configuration">Represents a set of key/value application configuration properties.</param>
    /// <param name="dbSession">Represents a session in the database/UOW that contains a connection and a transaction.</param>
    public CountryDeletedProjector(
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
        const string query = @"DELETE Countries WHERE IataCode = @iataCode";
        await _dbSession.Connection!.ExecuteAsync(query, new { iataCode }, _dbSession.Transaction);
    }

    #endregion
}
