#region Usings

using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Configuration;
using Records.Persons.Domain.Persons.Repositories;
using Records.Shared.Infra.Persistence;
using Records.Shared.Infra.Persistence.Abstractions;
using Records.Shared.Infra.Persistence.Mappings.Abstractions.Mappers;
using System.Collections.ObjectModel;
using DataModel = Records.Persons.Infra.Persistence.Sql.Persons.Models; // Using aliases.
using DomainModel = Records.Persons.Domain.Persons.Models; // Using aliases.

#endregion

namespace Records.Persons.Infra.Persistence.Sql.Persons.Repositories;

/// <summary>
/// Represents the persistence operations of the <see cref="DomainModel.Person"/>.
/// </summary>
public class PersonRepository : Repository, IPersonRepository
{
    #region Declarations

    /// <summary>Defines a mapper to map a <see cref="DomainModel.Person"/> to a <see cref="DataModel.Person"/> and vice versa.</summary>
    private readonly IPersistanceMapper<DomainModel.Person, DataModel.Person> _personMapper;

    #endregion

    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="PersonRepository"/> class.
    /// </summary>
    /// <param name="configuration">Represents a set of key/value application configuration properties.</param>
    /// <param name="dbSession">Represents a session in the database/UOW that contains a connection and a transaction.</param>
    /// <param name="personMapper">Defines a mapper to map a <see cref="DomainModel.Person"/> to a<see cref="DataModel.Person"/> and vice versa.</param>
    /// <exception cref="ArgumentNullException">When some argument for the constructor parameters is null.</exception>
    public PersonRepository(
        IConfiguration configuration,
        IDbSession dbSession,
        IPersistanceMapper<DomainModel.Person, DataModel.Person> personMapper)
        : base(configuration, dbSession)
    {
        _personMapper = personMapper ?? throw new ArgumentNullException(nameof(personMapper));
    }

    #endregion

    #region Public methods

    /// <inheritdoc />
    public async Task InsertAsync(DomainModel.Person domainPerson)
    {
        DataModel.Person dataPerson = _personMapper.FromDomainToDataModel(domainPerson);

        // Dapper.Contrib.
        await _dbSession.Connection.InsertAsync(dataPerson, _dbSession.Transaction);
        await _dbSession.Connection.InsertAsync(dataPerson.Address, _dbSession.Transaction);

        // NOTE: It requires ";MultipleActiveResultSets=True" in the connection string to allow multiple
        // active result sets when use the connection in parallel.
        ICollection<Task> tasks = new Collection<Task>();
        foreach (DataModel.PersonalAsset personalAsset in dataPerson.PersonalAssets!.NotNull())
        {
            tasks.Add(_dbSession.Connection.InsertAsync(personalAsset, _dbSession.Transaction));
        }

        await Task.WhenAll(tasks);

        // Dapper vanilla.
        /*
        string sql = @"INSERT INTO [dbo].[Persons]
                                ([Id]
                                ,[FullName]
                                ,[Email]
                                ,[Phone]
                                ,[Gender])
                            VALUES
                                (@id
                                ,@fullName
                                ,@email
                                ,@phone
                                ,@gender)";

        _dbSession.Connection.Execute(
            sql,
            new
            {
                id = dataPerson.Id,
                fullName = dataPerson.FullName,
                email = dataPerson.Email,
                phone = dataPerson.Phone,
                gender = dataPerson.Gender
            },
            _dbSession.Transaction);
        */

        ////_dbSession.Connection.Insert(dataPerson, _dbSession.Transaction); // ok (Dapper.Contrib).
        ////_dbSession.Connection.Insert(dataPerson, _dbSession.Transaction); // Dapper.Extensions.
        ////_dbSession.Connection.Insert<Guid, DataModel.Person>(dataPerson, _dbSession.Transaction); // ok (Dapper.SimpleCRUD).
    }

    /// <inheritdoc />
    public async Task UpdateAsync(DomainModel.Person domainPerson)
    {
        DataModel.Person dataPerson = _personMapper.FromDomainToDataModel(domainPerson);

        // Dapper.Contrib.
        await _dbSession.Connection.UpdateAsync(dataPerson, _dbSession.Transaction);
        await _dbSession.Connection.UpdateAsync(dataPerson.Address, _dbSession.Transaction);

        //// TODO: Make a specific Update function for PersonalAssets.
        ////ICollection<Task> tasks = new Collection<Task>();
        ////foreach (DataModel.PersonalAsset personalAsset in dataPerson.PersonalAssets!.NotNull())
        ////{
        ////    tasks.Add(_dbSession.Connection.UpdateAsync(personalAsset, _dbSession.Transaction));
        ////}
        ////await Task.WhenAll(tasks);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(DomainModel.Person domainPerson)
    {
        // Deletes the address.
        // Dapper.Contrib (with just PersonId). // It doesn't work, it only allows deleting by the PrimaryKey of the table.
        ////_dbSession.Connection.Delete<DataModel.Value>(new DataModel.Value { PersonId = domainPerson.Id }, _dbSession.Transaction);
        ////_dbSession.Connection.Delete<DataModel.Value>(new DataModel.Value { Id = domainPerson.Value.Id }, _dbSession.Transaction);

        // Deletes the address.
        // Dapper (native).
        const string sql1 = @"DELETE Addresses WHERE PersonId = @personId";
        await _dbSession.Connection!.ExecuteAsync(sql1, new { personId = domainPerson.Id }, _dbSession.Transaction);

        // Deletes personal assets.
        // Dapper (native).
        const string sql2 = @"DELETE PersonalAssets WHERE PersonId = @personId";
        await _dbSession.Connection!.ExecuteAsync(sql2, new { personId = domainPerson.Id }, _dbSession.Transaction);

        // Deletes personal assets.
        ////foreach (DomainModel.PersonalAsset personalAsset in domainPerson.PersonalAssets!.NotNull())
        ////{
        ////    _dbSession.Connection.Delete<DataModel.PersonalAsset>(new DataModel.PersonalAsset { PersonId = domainPerson.Id }, _dbSession.Transaction);
        ////}

        // Dapper (native).
        // Deletes the Person.
        await _dbSession.Connection.DeleteAsync<DataModel.Person>(new DataModel.Person { Id = domainPerson.Id }, _dbSession.Transaction);

        // Deletes the Person.
        // Dapper.Contrib (with full DataModel).
        ////DataModel.Person dataPerson = _personMapper.FromDomainToDataModel(domainPerson);
        ////_dbSession.Connection.Delete(dataPerson, _dbSession.Transaction);
    }

    /// <inheritdoc />
    public async Task<DomainModel.Person?> GetByIdAsync(Guid id)
    {
        ////using IDbConnection db = new SqlConnection(_sourceConnectionString);

        // Sql.
        const string query = @"
            SELECT PE.*, AD.*, PA.* 
            FROM Persons PE with (NOLOCK)
	            left join Addresses AD with (NOLOCK) on AD.PersonId = PE.Id
	            left join PersonalAssets PA with (NOLOCK) on PA.PersonId = PE.Id
            WHERE PE.Id = @id";

        IEnumerable<DataModel.Person>? personEnum = await _dbSession.Connection!.QueryAsync<
            DataModel.Person,
            DataModel.Address,
            DataModel.PersonalAsset,
            DataModel.Person>(
            query,
            (personAux, address, personalAsset) =>
            {
                personAux.Address = address;
                personAux.PersonalAssets!.Add(personalAsset);

                return personAux;
            },
            new
            {
                id = id,
            },
            splitOn: "Id,Id,Id");

        DataModel.Person? dataPerson = personEnum.GroupBy(p => p.Id).Select(g =>
        {
            DataModel.Person groupedPerson = g.First();
            groupedPerson.PersonalAssets = g.Select(p => p.PersonalAssets!.Single()).ToList();
            return groupedPerson;
        }).FirstOrDefault();

        return _personMapper.FromDataModelToDomain(dataPerson);
    }

    #endregion
}
