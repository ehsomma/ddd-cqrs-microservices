using System.Data;

namespace Records.Shared.Infra.Persistence.Abstractions;

/// <summary>
/// Represents a session in the database/UOW that contains a connection and a transaction.
/// </summary>
public interface IDbSession : IDisposable
{
    #region Properties

    /// <inheritdoc cref="IDbConnection"/>
    IDbConnection? Connection { get; }

    /// <inheritdoc cref="IDbTransaction"/>
    IDbTransaction? Transaction { get; set; }

    #endregion
}
