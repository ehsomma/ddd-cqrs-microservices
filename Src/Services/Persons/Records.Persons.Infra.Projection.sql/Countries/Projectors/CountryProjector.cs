#region Usings

using Dapper;
using Microsoft.Extensions.Configuration;
using My.Data.InterceptableDbConnection;
using Records.Shared.Infra.Projection;
using Records.Shared.Projection.Abstractions;
using System.Data;
using System.Data.SqlClient;
using ProjectionDataModel = Records.Persons.Infra.Projection.sql.Countries.Models;

#endregion

namespace Records.Persons.Infra.Projection.sql.Countries.Projectors;

/// <summary>
/// Represents a base class for Projectors of Country. Projectors project (copy) data from "source"
/// database to "projection" (read) database.
/// </summary>
public abstract class CountryProjector : Projector
{
    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CountryProjector"/> class.
    /// </summary>
    /// <param name="configuration">Represents a set of key/value application configuration properties.</param>
    /// <param name="dbSession">Represents a session in the database/UOW that contains a connection and a transaction.</param>
    /// <exception cref="ArgumentNullException">When some argument for the constructor parameters is null.</exception>
    protected CountryProjector(
        IConfiguration configuration,
        IDbSession dbSession)
        : base(configuration, dbSession)
    {
    }

    #endregion

    #region Private methods

    /// <summary>
    /// Read and map a ProjectionDataModel.Country from write (source) database.
    /// </summary>
    /// <param name="iataCode">The IATA code to search for.</param>
    /// <returns>A <see cref="ProjectionDataModel.Country"/> or null.</returns>
    protected async Task<ProjectionDataModel.Country?> GetCountryAsync(string iataCode)
    {
        // Database connection (source DataBase).
        ////using IDbConnection db = new SqlConnection(_sourceConnectionString);
        using IDbConnection db = new InterceptedDbConnection(new SqlConnection(_sourceConnectionString));

        const string query = $"SELECT * FROM Countries with (NOLOCK) WHERE IataCode = @iataCode";

        Records.Persons.Infra.Projection.sql.Countries.Models.Country? country =
            await db.QueryFirstOrDefaultAsync<Records.Persons.Infra.Projection.sql.Countries.Models.Country>(query, new { iataCode });

        return country;
    }

    #endregion
}
