#region Usings

using Microsoft.Extensions.Configuration;
using My.Data.InterceptableDbConnection;
using Records.Shared.Configuration;
using Records.Shared.Projection.Abstractions;
using System.Data;
using System.Data.SqlClient;
////using Records.Shared.Infra.Persistence.Abstractions; // IMPORTANT: Do not reference this.

#endregion

namespace Records.Shared.Infra.Projection;

/// <inheritdoc cref="IDbSession"/>
public sealed class DbSessionProjection : IDbSession
{
    #region Declarations

#pragma warning disable IDE0052 // Remove unread private members
    private readonly Guid _id;
#pragma warning restore IDE0052 // Remove unread private members

    #endregion

    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="DbSessionProjection"/> class.
    /// </summary>
    /// <param name="configuration">Represents a set of key/value application configuration properties.</param>
    public DbSessionProjection(IConfiguration configuration)
    {
        _id = Guid.NewGuid();

        SqlServerSettings databaseSettings = configuration.GetSectionOrThrow<SqlServerSettings>(SqlServerSettings.SettingsKey);

        // Connection to the "Projection" DataBase.
        // [!] Used as write database in this class (when we must write in the read database).
        ////Connection = new SqlConnection(databaseSettings.ProjectionConnectionString);
        Connection = new InterceptedDbConnection(new SqlConnection(databaseSettings.ProjectionConnectionString));

        Connection.Open(); // The running proces IP must have access to server.
    }

    #endregion

    #region Public methods

    /// <inheritdoc />
    public IDbConnection? Connection { get; }

    /// <inheritdoc />
    public IDbTransaction? Transaction { get; set; }

    /// <inheritdoc />
    public void Dispose()
    {
        Connection?.Dispose();
    }

    #endregion
}
