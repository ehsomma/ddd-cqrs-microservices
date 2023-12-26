#region Usings

using Microsoft.Extensions.Configuration;
using Records.Shared.Configuration;

#endregion

namespace Records.Persons.Infra.Reads.sql;

/// <summary>
/// Represents a base class for read repositories (reads from projection database).
/// </summary>
public class ReadRepository
{
    #region Declarations

    /// <summary>The connection string to the projection database.</summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.MaintainabilityRules",
        "SA1401:Fields should be private",
        Justification = "I prefer to use it as field just in protected fields in base classes like Repository base class.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Design",
        "CA1051:Do not declare visible instance fields",
        Justification = "I prefer to use it as field just in protected fields in base classes like Repository base class.")]
    //// ReSharper disable once InconsistentNaming
    protected readonly string? _connectionString;

    #endregion

    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="ReadRepository"/> class.
    /// </summary>
    /// <param name="configuration">Represents a set of key/value application configuration properties.</param>
    public ReadRepository(IConfiguration configuration)
    {
        SqlServerSettings databaseSettings = configuration.GetSectionOrThrow<SqlServerSettings>(SqlServerSettings.SettingsKey);
        ////_connectionString = databaseSettings.SourceConnectionString;
        _connectionString = databaseSettings.ProjectionConnectionString;
    }

    #endregion
}
