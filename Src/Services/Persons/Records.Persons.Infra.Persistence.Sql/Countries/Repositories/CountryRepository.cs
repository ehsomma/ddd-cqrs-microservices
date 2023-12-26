#region Usings

using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Configuration;
using Records.Persons.Domain.Countries.Repositories;
using Records.Persons.Domain.Countries.ValueObjects;
using Records.Persons.Domain.Shared.ValueObjects;
using Records.Shared.Infra.Persistence;
using Records.Shared.Infra.Persistence.Abstractions;
using Records.Shared.Infra.Persistence.Mappings.Abstractions.Mappers;
using DataModel = Records.Persons.Infra.Persistence.Sql.Countries.Models; // Using aliases.
using DomainModel = Records.Persons.Domain.Countries.Models; // Using aliases.

#endregion

namespace Records.Persons.Infra.Persistence.Sql.Countries.Repositories;

/// <summary>
/// Represents the <see cref="DomainModel.Country"/> persistence operations.
/// </summary>
public class CountryRepository : Repository, ICountryRepository
{
    #region Declarations

    /// <summary>Defines a mapper to map a <see cref="DomainModel.Country"/> to a <see cref="DataModel.Country"/> and vice versa.</summary>
    private readonly IPersistanceMapper<DomainModel.Country, DataModel.Country> _countryMapper;

    #endregion

    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CountryRepository"/> class.
    /// </summary>
    /// <param name="configuration">Represents a set of key/value application configuration properties.</param>
    /// <param name="dbSession">Represents a session in the database/UOW that contains a connection and a transaction.</param>
    /// <param name="countryMapper">Defines a mapper to map a <see cref="DomainModel.Country"/> to a<see cref="DataModel.Country"/> and vice versa.</param>
    /// <exception cref="ArgumentNullException">When some argument for the constructor parameters is null.</exception>
    public CountryRepository(
        IConfiguration configuration,
        IDbSession dbSession,
        IPersistanceMapper<DomainModel.Country, DataModel.Country> countryMapper)
        : base(configuration, dbSession)
    {
        _countryMapper = countryMapper ?? throw new ArgumentNullException(nameof(countryMapper));
    }

    #endregion

    #region Public methods

    /// <inheritdoc />
    public async Task InsertAsync(DomainModel.Country domainCountry)
    {
        DataModel.Country dataCountry = _countryMapper.FromDomainToDataModel(domainCountry);

        await _dbSession.Connection.InsertAsync(dataCountry, _dbSession.Transaction);
    }

    /// <inheritdoc />
    public async Task UpdateAsync(DomainModel.Country domainCountry)
    {
        DataModel.Country dataCountry = _countryMapper.FromDomainToDataModel(domainCountry);

        await _dbSession.Connection.UpdateAsync(dataCountry, _dbSession.Transaction);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(DomainModel.Country domainCountry)
    {
        DataModel.Country dataCountry = _countryMapper.FromDomainToDataModel(domainCountry);

        await _dbSession.Connection.DeleteAsync(dataCountry, _dbSession.Transaction);
    }

    /// <inheritdoc />
    public async Task<DomainModel.Country?> GetByIataCodeAsync(CountryIataCode iataCode)
    {
        const string Query = "SELECT * FROM Countries with (NOLOCK) WHERE IataCode = @iataCode";

        DataModel.Country? dataCountry =
            await _dbSession.Connection!.QueryFirstOrDefaultAsync<DataModel.Country>(Query, new { iataCode });

        return _countryMapper.FromDataModelToDomain(dataCountry);
    }

    /// <inheritdoc />
    public async Task<bool> Exists(CountryIataCode iataCode)
    {
        const string Query = "SELECT COUNT(1) FROM Countries WHERE IataCode = @iataCode";
        bool exists = await _dbSession.Connection!.ExecuteScalarAsync<bool>(Query, new { iataCode });

        return exists;
    }

    /// <inheritdoc />
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "ValueObject")]
    public async Task<bool> Exists(CountryName countryName)
    {
        const string Query = "SELECT COUNT(1) FROM Countries WHERE Name = @name";
        bool exists = await _dbSession.Connection!
            .ExecuteScalarAsync<bool>(Query, new { name = countryName.Value });

        return exists;
    }

    #endregion
}
