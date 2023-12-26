#region Usings

using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Configuration;
using Records.Persons.Projection.Persons.Projectors;
using Records.Shared.Projection.Abstractions;
using ProjectionDataModel = Records.Persons.Infra.Projection.sql.Persons.Models;

#endregion

namespace Records.Persons.Infra.Projection.sql.Persons.Projectors;

/// <inheritdoc cref="IPersonDeletedProjector"/>
public class PersonDeletedProjector : PersonProjector, IPersonDeletedProjector
{
    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="PersonDeletedProjector"/> class.
    /// </summary>
    /// <param name="configuration">Represents a set of key/value application configuration properties.</param>
    /// <param name="dbSession">Represents a session in the database/UOW that contains a connection and a transaction.</param>
    public PersonDeletedProjector(
        IConfiguration configuration,
        IDbSession dbSession)
        : base(configuration, dbSession)
    {
    }

    #endregion

    #region Public methods

    /// <inheritdoc />
    public async Task ProjectAsync(Guid id)
    {
        await DeletePersonAsync(id);
    }

    #endregion

    #region Private methods

    private async Task DeletePersonAsync(Guid id)
    {
        // Deletes the address.
        const string sql1 = @"DELETE Addresses WHERE PersonId = @personId";
        await _dbSession.Connection!.ExecuteAsync(sql1, new { personId = id }, _dbSession.Transaction);

        // Deletes personal assets.
        const string sql2 = @"DELETE PersonalAssets WHERE PersonId = @personId";
        await _dbSession.Connection!.ExecuteAsync(sql2, new { personId = id }, _dbSession.Transaction);

        // Deletes the Person.
        await _dbSession.Connection.DeleteAsync<ProjectionDataModel.Person>(new ProjectionDataModel.Person { Id = id }, _dbSession.Transaction);

        // Deletes the PersonSummary.
        await _dbSession.Connection.DeleteAsync<ProjectionDataModel.PersonSummary>(new ProjectionDataModel.PersonSummary { Id = id }, _dbSession.Transaction);
    }

    #endregion
}
