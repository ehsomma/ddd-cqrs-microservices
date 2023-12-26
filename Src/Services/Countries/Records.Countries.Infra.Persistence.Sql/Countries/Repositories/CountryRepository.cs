#region Usings

using Microsoft.Extensions.Configuration;
using Records.Countries.Domain.Countries.Repositories;
using Records.Shared.Infra.Persistence;
using Records.Shared.Infra.Persistence.Abstractions;

#endregion

namespace Records.Countries.Infra.Persistence.Sql.Countries.Repositories;

/// <summary>
/// Manages the persistence operations of <see cref="Countries"/>.
/// </summary>
public class CountryRepository : Repository, ICountryRepository
{
    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CountryRepository"/> class.
    /// </summary>
    /// <param name="configuration">Represents a set of key/value application configuration properties.</param>
    /// <param name="dbSession">Represents a session in the database/UOW that contains a connection and a transaction.</param>
    public CountryRepository(IConfiguration configuration, IDbSession dbSession)
        : base(configuration, dbSession)
    {
        // TODO: Implement.
    }

    #endregion
}
