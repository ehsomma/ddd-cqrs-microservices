namespace Records.Shared.Infra.Persistence.Abstractions;

/// <summary>
/// Manage a <see cref="IDbSession"/> to encapsulate a business transaction which can affect the database.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    #region Private methods

    /// <summary>
    /// Begins a database transaction.
    /// </summary>
    void BeginTransaction();

    /// <summary>
    /// Commits the database transaction.
    /// </summary>
    void Commit();

    /// <summary>
    /// Rolls back a transaction from a pending state.
    /// </summary>
    void Rollback();

    #endregion
}
