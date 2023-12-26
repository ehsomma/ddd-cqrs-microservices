#region Usings

using System.Data;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Transactions;

#endregion

namespace My.Data.InterceptableDbConnection;

/// <summary>
/// Represents a wrapper around IDbConnection, which allows us to build a wrapped IDbCommand for
/// logging/debugging or catch commands exceptions.
/// </summary>
public class InterceptedDbConnection : DbConnection
{
    #region Declarations

    private DbConnection _connection;

    #endregion

    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="InterceptedDbConnection"/> class.
    /// </summary>
    /// <param name="connection">The connection to wrap.</param>
    /// <exception cref="ArgumentNullException">When some argument for the constructor parameters is null.</exception>
    public InterceptedDbConnection(DbConnection connection)
    {
        _connection = connection ?? throw new ArgumentNullException(nameof(connection));
    }

    #endregion

    #region Properties

    /// <inheritdoc cref="DbConnection.ConnectionString"/>
    [AllowNull]
    public override string ConnectionString
    {
        get => _connection.ConnectionString;
        set => _connection.ConnectionString = value;
    }

    /// <inheritdoc cref="DbConnection.ConnectionTimeout"/>
    public override int ConnectionTimeout => _connection.ConnectionTimeout;

    /// <inheritdoc cref="DbConnection.Database"/>
    public override string Database => _connection.Database;

    /// <inheritdoc cref="DbConnection.DataSource"/>
    public override string DataSource => _connection.DataSource;

    /// <inheritdoc cref="DbConnection.ServerVersion"/>
    public override string ServerVersion => _connection.ServerVersion;

    /// <inheritdoc cref="DbConnection.State"/>
    public override ConnectionState State => _connection.State;

    /// <summary>
    /// Gets a value indicating whether events can be raised.
    /// </summary>
    protected override bool CanRaiseEvents => true;

    #endregion

    #region Public methods

    /// <inheritdoc cref="DbConnection.ChangeDatabase(string)"/>
    public override void ChangeDatabase(string databaseName)
    {
        _connection.ChangeDatabase(databaseName);
    }

    /// <inheritdoc cref="DbConnection.Close()"/>
    public override void Close()
    {
        _connection.Close();
    }

    /// <inheritdoc cref="DbConnection.Open()"/>
    public override void Open()
    {
        _connection.Open();
    }

    /// <inheritdoc cref="DbConnection.OpenAsync(CancellationToken)"/>
    public override async Task OpenAsync(CancellationToken cancellationToken)
    {
        await _connection.OpenAsync(cancellationToken);
    }

    /// <inheritdoc cref="DbConnection.EnlistTransaction(System.Transactions.Transaction)"/>
    public override void EnlistTransaction(Transaction? transaction)
    {
        _connection.EnlistTransaction(transaction);
    }

    /// <inheritdoc cref="DbConnection.GetSchema()"/>
    public override DataTable GetSchema()
    {
        return _connection.GetSchema();
    }

    /// <inheritdoc cref="DbConnection.GetSchema(string)"/>
    public override DataTable GetSchema(string collectionName)
    {
        return _connection.GetSchema(collectionName);
    }

    /// <inheritdoc cref="DbConnection.GetSchema(string, string[])"/>
    public override DataTable GetSchema(string collectionName, string?[] restrictionValues)
    {
        return _connection.GetSchema(collectionName, restrictionValues);
    }

    #endregion

    #region Private methods

    /// <inheritdoc cref="DbConnection.BeginDbTransaction(System.Data.IsolationLevel)"/>
    protected override DbTransaction BeginDbTransaction(System.Data.IsolationLevel isolationLevel)
    {
        return _connection.BeginTransaction(isolationLevel);
    }

    /// <inheritdoc cref="DbConnection.CreateDbCommand()"/>
    protected override DbCommand CreateDbCommand()
    {
        // Wrap original command under InterceptedDbCommand.
        DbCommand originalCommand = _connection.CreateCommand();
        return new InterceptedDbCommand(originalCommand, this);
    }

    /// <summary>
    /// Dispose the underlying connection.
    /// </summary>
    /// <param name="disposing">False if preempted from a finalizer.</param>
    protected override void Dispose(bool disposing)
    {
        ////if (disposing && _connection != null)
        if (disposing)
        {
            _connection.StateChange -= StateChangeHandler;
            _connection.Dispose();
        }

        base.Dispose(disposing);

        _connection = null!;
    }

    /// <summary>
    /// The state change handler.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="stateChangeEventArguments">The state change event arguments.</param>
    private void StateChangeHandler(object sender, StateChangeEventArgs stateChangeEventArguments)
    {
        OnStateChange(stateChangeEventArguments);
    }

    #endregion
}
