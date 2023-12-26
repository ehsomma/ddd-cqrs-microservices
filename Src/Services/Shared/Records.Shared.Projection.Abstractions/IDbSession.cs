using System.Data;

namespace Records.Shared.Projection.Abstractions;

/// <summary>
/// Represents a session in the database/UOW that contains a connection and a transaction (for projections).
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
