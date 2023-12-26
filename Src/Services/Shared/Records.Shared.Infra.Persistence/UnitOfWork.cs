using Records.Shared.Infra.Persistence.Abstractions;

namespace Records.Shared.Infra.Persistence;

/// <inheritdoc cref="IUnitOfWork"/>
public sealed class UnitOfWork : IUnitOfWork
{
    #region Declarations

    private readonly IDbSession _session;

    #endregion

    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
    /// </summary>
    /// <param name="session">Represents a session in the database/UOW that contains a connection and a transaction (for projections).</param>
    /// <exception cref="ArgumentNullException">When some argument for the constructor parameters is null.</exception>
    public UnitOfWork(IDbSession session)
    {
        _session = session ?? throw new ArgumentNullException(nameof(session));
    }

    #endregion

    #region Public methods

    /// <inheritdoc />
    public void BeginTransaction()
    {
        _session.Transaction = _session.Connection?.BeginTransaction();
    }

    /// <inheritdoc />
    public void Commit()
    {
        _session.Transaction?.Commit();
        Dispose();
    }

    /// <inheritdoc />
    public void Rollback()
    {
        _session.Transaction?.Rollback();
        Dispose();
    }

    /// <inheritdoc />
    public void Dispose()
    {
        _session.Transaction?.Dispose();
    }

    #endregion
}
