#region Usings

using Dapper;
using Microsoft.Extensions.Configuration;
using My.Data.InterceptableDbConnection;
using Records.Persons.Reads.Persons.GetPersonById;
////using ReadModel = Records.Persons.Reads.Persons.Models; // Using aliases. Exception to the "Using aliases" policy.
using Records.Persons.Reads.Persons.Models;
using System.Data;
using System.Data.SqlClient;

#endregion

namespace Records.Persons.Infra.Reads.sql.Persons.GetPersonById;

/// <summary>
/// Represents a read repository (reads from projection database).
/// </summary>
public class GetPersonByIdRepository : ReadRepository, IGetPersonByIdRepository ////IReadRepository<Guid, ReadModel.Person>
{
    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="GetPersonByIdRepository"/> class.
    /// </summary>
    /// <param name="configuration">Represents a set of key/value application configuration properties.</param>
    public GetPersonByIdRepository(IConfiguration configuration)
        : base(configuration)
    {
    }

    #endregion

    #region Public methods

    /// <inheritdoc />
    public async Task<Person?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        // Database connection.
        ////using IDbConnection db = new SqlConnection(_connectionString);
        using IDbConnection db = new InterceptedDbConnection(new SqlConnection(_connectionString));

        // Sql.
        const string sql = @"
            SELECT PE.*, AD.*, PA.* 
            FROM Persons PE with (NOLOCK)
	            left join Addresses AD with (NOLOCK) on AD.PersonId = PE.Id
	            left join PersonalAssets PA with (NOLOCK) on PA.PersonId = PE.Id
            WHERE PE.Id = @id";

        // This way we can use cancellationToken.
        CommandDefinition cmd = new (sql, new { id }, cancellationToken: cancellationToken);

        IEnumerable<Person>? personEnum = await db.QueryAsync<
            Person,
            Address,
            LatLng,
            PersonalAsset,
            Person>(
            cmd,
            (personAux, address, latLng, personalAsset) =>
            {
                address.LatLng = latLng;
                personAux.Address = address;
                personAux.PersonalAssets!.Add(personalAsset);

                return personAux;
            },
            splitOn: "Id,Id,Lat,Id");

        /*
        // This way we can't use cancellationToken.
        IEnumerable<Person>? personEnum = await db.QueryAsync<
                Person,
                Value,
                LatLng,
                PersonalAsset,
                Person>(
            sql,
            (personAux, address, latLng, personalAsset) =>
            {
                address.LatLng = latLng;
                personAux.Value = address;
                personAux.PersonalAssets!.Add(personalAsset);

                return personAux;
            },
            new
            {
                id
            },
            splitOn: "Id,Id,Lat,Id");
        */

        Person? person = personEnum.GroupBy(p => p.Id).Select(g =>
        {
            Person groupedPerson = g.First();
            groupedPerson.PersonalAssets = g.Select(p => p.PersonalAssets!.Single()).ToList();
            return groupedPerson;
        }).FirstOrDefault();

        return person;
    }

    #endregion
}
