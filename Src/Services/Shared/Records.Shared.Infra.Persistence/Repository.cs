#region Usings

using Microsoft.Extensions.Configuration;
using Records.Shared.Configuration;
using Records.Shared.Infra.Persistence.Abstractions;

#endregion

namespace Records.Shared.Infra.Persistence;

/// <summary>
/// Represents a base class for repositories.
/// </summary>
public abstract class Repository
{
    #region Declarations

    /// <inheritdoc cref="IDbSession"/>
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.MaintainabilityRules",
        "SA1401:Fields should be private",
        Justification = "I prefer to use it as field just in protected fields in base classes like Repository base class.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Design",
        "CA1051:Do not declare visible instance fields",
        Justification = "I prefer to use it as field just in protected fields in base classes like Repository base class.")]
    //// ReSharper disable once InconsistentNaming
    protected readonly IDbSession _dbSession;
    ////public IDbSession DbSession { get; init; }

    // Used to read from the main database.
    ////protected readonly string? _sourceConnectionString;

    #endregion

    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="Repository"/> class.
    /// </summary>
    /// <param name="configuration">Represents a set of key/value application configuration properties.</param>
    /// <param name="dbSession">Represents a session in the database/UOW that contains a connection and a transaction.</param>
    /// <exception cref="ArgumentNullException">When some argument for the constructor parameters is null.</exception>
    protected Repository(
        IConfiguration configuration,
        IDbSession dbSession)
    {
        _dbSession = dbSession ?? throw new ArgumentNullException(nameof(dbSession));

#pragma warning disable IDE0059 // Unnecessary assignment of a value
        SqlServerSettings databaseSettings = configuration.GetSectionOrThrow<SqlServerSettings>(SqlServerSettings.SettingsKey);
#pragma warning restore IDE0059 // Unnecessary assignment of a value

        // Source database used to read from the "Source" and then project to the "Projection" database.
        ////_sourceConnectionString = databaseSettings.SourceConnectionString;
    }

    #endregion
}
