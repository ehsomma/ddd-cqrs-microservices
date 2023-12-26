#region Usings

using System.Data;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Text;

#endregion

namespace My.Data.InterceptableDbConnection;

/// <summary>
/// Represents a wrapper around IDbCommand, which allows us to log queries or inspect how Dapper is
/// passing our Parameters and catch commands exceptions.
/// </summary>
public class InterceptedDbCommand : DbCommand
{
    #region Declarations

    private DbCommand _command;
    private DbConnection? _connection;
    private DbTransaction? _transaction;
    private string? _commandText;
    private string? _commandParamsSummary;

    #endregion

    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="InterceptedDbCommand"/> class.
    /// </summary>
    /// <param name="command">The command to wrap.</param>
    /// <param name="connection">The connection.</param>
    /// <exception cref="ArgumentNullException">When some argument for the constructor parameters is null.</exception>
    public InterceptedDbCommand(DbCommand command, DbConnection? connection)
    {
        _command = command ?? throw new ArgumentNullException(nameof(command));

        if (connection != null)
        {
            _connection = connection;
        }
    }

    #endregion

    #region Properties

    /// <inheritdoc cref="DbCommand.CommandText"/>
    [AllowNull]
    [SuppressMessage(
        "Security",
        "CA2100:Review SQL queries for security vulnerabilities",
        Justification = "We use params.")]
    public override string CommandText
    {
        get => _command.CommandText;
        set
        {
            _command.CommandText = value;
            _commandText = value;
        }
    }

    /// <inheritdoc cref="DbCommand.CommandTimeout"/>
    public override int CommandTimeout
    {
        get => _command.CommandTimeout;
        set => _command.CommandTimeout = value;
    }

    /// <inheritdoc cref="DbCommand.CommandType"/>
    public override CommandType CommandType
    {
        get => _command.CommandType;
        set => _command.CommandType = value;
    }

    /// <inheritdoc cref="DbCommand.DesignTimeVisible"/>
    public override bool DesignTimeVisible
    {
        get => _command.DesignTimeVisible;
        set => _command.DesignTimeVisible = value;
    }

    /// <inheritdoc cref="DbCommand.UpdatedRowSource"/>
    public override UpdateRowSource UpdatedRowSource
    {
        get => _command.UpdatedRowSource;
        set => _command.UpdatedRowSource = value;
    }

    /// <inheritdoc cref="DbCommand.DbConnection"/>
    protected override DbConnection? DbConnection
    {
        get => _connection;
        set => _connection = value;
    }

    /// <inheritdoc cref="DbCommand.DbParameterCollection"/>
    protected override DbParameterCollection DbParameterCollection => _command.Parameters;

    /// <inheritdoc cref="DbCommand.DbTransaction"/>
    protected override DbTransaction? DbTransaction
    {
        get => _transaction;
        set
        {
            _command.Transaction = value;
            _transaction = value;
        }
    }

    #endregion

    #region Public methods

    /// <inheritdoc cref="DbCommand.Prepare()"/>
    public override void Prepare()
    {
        _command.Prepare();
    }

    /// <inheritdoc cref="DbCommand.Cancel()"/>
    public override void Cancel()
    {
        _command.Cancel();
    }

    /// <inheritdoc cref="DbCommand.ExecuteNonQuery()"/>
    public override int ExecuteNonQuery()
    {
        try
        {
            return _command.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            // Adds _cmd.CommandText and _cmd.Parameters (name, type and value) to the exception.
            AddDataToException(ex);
            throw;
        }
    }

    /// <inheritdoc cref="DbCommand.ExecuteNonQueryAsync(CancellationToken)"/>
    public override async Task<int> ExecuteNonQueryAsync(CancellationToken cancellationToken)
    {
        try
        {
            return await _command.ExecuteNonQueryAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            // Adds _command.CommandText and _command.Parameters (name, type and value) to the exception.
            AddDataToException(ex);
            throw;
        }
    }

    /// <inheritdoc cref="DbCommand.ExecuteScalar()"/>
    public override object? ExecuteScalar()
    {
        try
        {
            return _command.ExecuteScalar();
        }
        catch (Exception ex)
        {
            // Adds _command.CommandText and _command.Parameters (name, type and value) to the exception.
            AddDataToException(ex);
            throw;
        }
    }

    /// <inheritdoc cref="DbCommand.ExecuteScalarAsync(CancellationToken)"/>
    public override async Task<object?> ExecuteScalarAsync(CancellationToken cancellationToken)
    {
        try
        {
            return await _command.ExecuteScalarAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            // Adds _command.CommandText and _command.Parameters (name, type and value) to the exception.
            AddDataToException(ex);
            throw;
        }
    }

    #endregion

    #region Private methods

    /// <inheritdoc cref="DbCommand.ExecuteDbDataReader(CommandBehavior)"/>
    protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
    {
        try
        {
            return _command.ExecuteReader(behavior);
        }
        catch (Exception ex)
        {
            // Adds _command.CommandText and _command.Parameters (name, type and value) to the exception.
            AddDataToException(ex);
            throw;
        }
    }

    /// <inheritdoc cref="DbCommand.ExecuteDbDataReaderAsync(CommandBehavior, CancellationToken)"/>
    protected override async Task<DbDataReader> ExecuteDbDataReaderAsync(CommandBehavior behavior, CancellationToken cancellationToken)
    {
        try
        {
            return await _command.ExecuteReaderAsync(behavior, cancellationToken);
        }
        catch (Exception ex)
        {
            // Adds _command.CommandText and _command.Parameters (name, type and value) to the exception.
            AddDataToException(ex);
            throw;
        }
    }

    /// <inheritdoc cref="DbCommand.CreateDbParameter()"/>
    protected override DbParameter CreateDbParameter()
    {
        return _command.CreateParameter();
    }

    /// <summary>
    /// Releases all resources used by this command.
    /// </summary>
    /// <param name="disposing">false if this is being disposed in a <c>finalizer</c>.</param>
    protected override void Dispose(bool disposing)
    {
        ////if (disposing && _command != null)
        if (disposing)
        {
            _command.Dispose();
        }

        _command = null!;

        base.Dispose(disposing);
    }

    /// <summary>
    /// Adds _cmd.CommandText and _cmd.Parameters (name, type and value) to the exception.
    /// </summary>
    /// <param name="ex">The exception.</param>
    private void AddDataToException(Exception ex)
    {
        if (_command.Parameters.Count > 0)
        {
            StringBuilder myparamsBuilder = new ();
            foreach (DbParameter param in _command.Parameters)
            {
                myparamsBuilder.AppendLine($"{param.ParameterName} ({param.DbType.ToString()}): {param.Value}\n");
            }

            _commandParamsSummary = myparamsBuilder.ToString();
        }

        ex.Data["commandText"] = _commandText;
        ex.Data["commandParamsSummary"] = _commandParamsSummary;
        ex.Data["database"] = _command.Connection?.Database;
        ex.Data["dataSource"] = _command.Connection?.DataSource;
    }

    #endregion
}
