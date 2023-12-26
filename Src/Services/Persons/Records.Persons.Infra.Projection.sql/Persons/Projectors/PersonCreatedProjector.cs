#region Usings

using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Configuration;
using Records.Persons.Projection.Persons.Projectors;
using Records.Shared.Projection.Abstractions;
using ProjectionDataModel = Records.Persons.Infra.Projection.sql.Persons.Models;

#endregion

namespace Records.Persons.Infra.Projection.sql.Persons.Projectors;

/// <inheritdoc cref="IPersonCreatedProjector"/>
public class PersonCreatedProjector : PersonProjector, IPersonCreatedProjector
{
    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="PersonCreatedProjector"/> class.
    /// </summary>
    /// <param name="configuration">Represents a set of key/value application configuration properties.</param>
    /// <param name="dbSession">Represents a session in the database/UOW that contains a connection and a transaction.</param>
    public PersonCreatedProjector(
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
        // Read ProjectionDataModel.Person from source repository.
        ProjectionDataModel.Person? person = await GetPersonAsync(id);

        if (person != null)
        {
            // Write (project) ProjectionDataModel.Person into projection repository.
            await InsertPersonAsync(person);

            // Write (project) ProjectionDataModel.PersonSummary into projection repository.
            ProjectionDataModel.PersonSummary personSummary = BuildPersonSummary(person);
            await InsertPersonSummaryAsync(personSummary);
        }
    }

    #endregion

    #region Private methods

    private async Task InsertPersonAsync(ProjectionDataModel.Person person)
    {
        await _dbSession.Connection.InsertAsync(person, _dbSession.Transaction);
        await _dbSession.Connection.InsertAsync(person.Address, _dbSession.Transaction);

        foreach (ProjectionDataModel.PersonalAsset personalAsset in person.PersonalAssets!.NotNull())
        {
            await _dbSession.Connection.InsertAsync(personalAsset, _dbSession.Transaction);
        }
    }

    private async Task InsertPersonSummaryAsync(ProjectionDataModel.PersonSummary personSummary)
    {
        await _dbSession.Connection.InsertAsync(personSummary, _dbSession.Transaction);
    }

    #endregion
}
