#region Usings

using Dapper;
using Microsoft.Extensions.Configuration;
using My.Data.InterceptableDbConnection;
using Records.Persons.Infra.Projection.sql.Persons.Models;
////using Records.Shared.Infra.Persistence.Abstractions;
using Records.Shared.Infra.Projection;
using Records.Shared.Projection.Abstractions;
using System.Data;
using System.Data.SqlClient;
////using ReadModel = Records.Persons.Reads.Persons.Models;
using ProjectionDataModel = Records.Persons.Infra.Projection.sql.Persons.Models;

#endregion

namespace Records.Persons.Infra.Projection.sql.Persons.Projectors;

/// <summary>
/// Represents a base class for Projectors of Person. Projectors project (copy) data from "source"
/// database to "projection" (read) database.
/// </summary>
public abstract class PersonProjector : Projector
{
    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="PersonProjector"/> class.
    /// </summary>
    /// <param name="configuration">Represents a set of key/value application configuration properties.</param>
    /// <param name="dbSession">Represents a session in the database/UOW that contains a connection and a transaction.</param>
    protected PersonProjector(
        IConfiguration configuration,
        IDbSession dbSession)
        : base(configuration, dbSession)
    {
    }

    #endregion

    #region Private methods

    /// <summary>
    /// Read and map a ProjectionDataModel.Person from write (source) database.
    /// </summary>
    /// <param name="personId">The person ID to search for.</param>
    /// <returns>A <see cref="ProjectionDataModel.Person"/> or null.</returns>
    protected async Task<ProjectionDataModel.Person?> GetPersonAsync(Guid personId)
    {
        // Database connection (source DataBase).
        ////using IDbConnection db = new SqlConnection(_sourceConnectionString);
        using IDbConnection db = new InterceptedDbConnection(new SqlConnection(_sourceConnectionString));

        // Sql.
        string sql = @"
            SELECT PE.*, AD.*, PA.* 
            FROM Persons PE with (NOLOCK)
	            left join Addresses AD with (NOLOCK) on AD.PersonId = PE.Id
	            left join PersonalAssets PA with (NOLOCK) on PA.PersonId = PE.Id
            WHERE PE.Id = @id";

        IEnumerable<ProjectionDataModel.Person>? personEnum = await db.QueryAsync<
            Person,
            Address,
            PersonalAsset,
            Person>(
            sql,
            (personAux, address, personalAsset) =>
            {
                personAux.Address = address;
                personAux.PersonalAssets!.Add(personalAsset);

                return personAux;
            },
            new
            {
                id = personId,
            },
            splitOn: "Id,Id,Id");

        ProjectionDataModel.Person? person = personEnum.GroupBy(p => p.Id).Select(g =>
        {
            ProjectionDataModel.Person groupedPerson = g.First();
            groupedPerson.PersonalAssets = g.Select(p => p.PersonalAssets!.Single()).ToList();
            return groupedPerson;
        }).FirstOrDefault();

        return person;
    }

    /// <summary>
    /// Flats the full Person object in a summary object.
    /// </summary>
    /// <param name="person">A full person object to get the data.</param>
    /// <returns>A <see cref="ProjectionDataModel.PersonSummary"/>.</returns>
    protected ProjectionDataModel.PersonSummary BuildPersonSummary(ProjectionDataModel.Person person)
    {
        ArgumentNullException.ThrowIfNull(person);

        decimal personalAssetsCount = 0;
        decimal personalAssetsBalance = 0;

        if (person.PersonalAssets != null)
        {
            personalAssetsCount = person.PersonalAssets.Count;
            personalAssetsBalance = person.PersonalAssets.Sum(pa => pa.Value);
        }

        ProjectionDataModel.PersonSummary personSummary = new ()
        {
            Id = person.Id,
            FullName = person.FullName,
            Email = person.Email,
            Phone = person.Phone,
            Gender = person.Gender,
            Birthdate = person.Birthdate,
            City = person.Address?.City,
            State = person.Address?.State,
            Country = person.Address?.Country,
            Lat = person.Address?.Lat,
            Lng = person.Address?.Lng,
            PersonalAssetsCount = personalAssetsCount,
            PersonalAssetsBalance = personalAssetsBalance,
        };

        return personSummary;
    }

    #endregion
}
