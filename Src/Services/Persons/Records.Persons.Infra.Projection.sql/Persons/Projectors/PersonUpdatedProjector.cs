#region Usings

using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Configuration;
using Records.Persons.Projection.Persons.Projectors;
using Records.Shared.Projection.Abstractions;
using System.Collections.ObjectModel;
using ProjectionDataModel = Records.Persons.Infra.Projection.sql.Persons.Models;

#endregion

namespace Records.Persons.Infra.Projection.sql.Persons.Projectors;

/// <inheritdoc cref="IPersonUpdatedProjector"/>
public class PersonUpdatedProjector : PersonProjector, IPersonUpdatedProjector
{
    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="PersonUpdatedProjector"/> class.
    /// </summary>
    /// <param name="configuration">Represents a set of key/value application configuration properties.</param>
    /// <param name="dbSession">Represents a session in the database/UOW that contains a connection and a transaction.</param>
    public PersonUpdatedProjector(
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
        /*
        Obsolet Message issue:
        Example:
        Microservice 1
        MSG_1 with entity #1 (updated "born" 10:00) => Publish Fail => reintent in 5'' => Publish ok
        MSG_2 with entity #1 (updated "grew" 10:05) => Publish ok

        Microservice 2
        Consume MSG_2 ok, entity #1 update to "grew" (u10:05)
        Consume MSG_1 ok, entity #1 update to "born" (u10:01) [!] Obsolet!

        NOTE: In the proyection we don't have this issue because here we always read the
        last version from the source database.
        */

        // Read Person ProjectionDataModel from source repository.
        ProjectionDataModel.Person? person = await GetPersonAsync(id);

        if (person != null)
        {
            // Write (project) ProjectionDataModel.Person into projection repository.
            await UpdatePersonAsync(person);

            // Write (project) ProjectionDataModel.PersonSummary into projection repository.
            ProjectionDataModel.PersonSummary personSummary = BuildPersonSummary(person);
            await UpdatePersonSummary(personSummary);
        }
    }

    #endregion

    #region Private methods

    private async Task UpdatePersonAsync(ProjectionDataModel.Person person)
    {
        await _dbSession.Connection.UpdateAsync(person, _dbSession.Transaction);
        await _dbSession.Connection.UpdateAsync(person.Address, _dbSession.Transaction);

        ////foreach (ProjectionDataModel.PersonalAsset personalAsset in person.PersonalAssets!.NotNull())
        ////{
        ////    await _dbSession.Connection.UpdateAsync(personalAsset, _dbSession.Transaction);
        ////}

        // NOTE: It requires ";MultipleActiveResultSets=True" in the connection string to allow multiple
        // active result sets when use the connection in paralel.
        ICollection<Task> tasks = new Collection<Task>();
        foreach (ProjectionDataModel.PersonalAsset personalAsset in person.PersonalAssets!.NotNull())
        {
            tasks.Add(_dbSession.Connection.UpdateAsync(personalAsset, _dbSession.Transaction));
        }

        await Task.WhenAll(tasks);
    }

    private async Task UpdatePersonSummary(ProjectionDataModel.PersonSummary personSummary)
    {
        await _dbSession.Connection.UpdateAsync(personSummary, _dbSession.Transaction);
    }

    #endregion
}
