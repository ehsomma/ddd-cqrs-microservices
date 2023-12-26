#region Usings

using Dapper;
using Microsoft.Extensions.Configuration;
using Records.Shared.Infra.Persistence;
using Records.Shared.Infra.Persistence.Abstractions;
using Records.Shared.Messaging;

#endregion

namespace Records.Shared.Infra.Outbox.Sql.Repositories;

/// <summary>
/// Manages the persistence operations of the <see cref="Message{TContent}"/> to implement the Outbox pattern.
/// </summary>
public class OutboxRepository : Repository, IOutboxRepository
{
    #region Declarations

    private readonly IOutboxMapper _outboxMapper;

    #endregion

    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="OutboxRepository"/> class.
    /// </summary>
    /// <param name="configuration">Represents a set of key/value application configuration properties.</param>
    /// <param name="dbSession">Represents a session in the database/UOW that contains a connection and a transaction.</param>
    /// <param name="outboxMapper">Represent a mapper to map <see cref="Message{TContent}" /> to <see cref="OutboxMessage" />.</param>
    /// <exception cref="ArgumentNullException">When some argument for the constructor parameters is null.</exception>
    public OutboxRepository(
        IConfiguration configuration,
        IDbSession dbSession,
        IOutboxMapper outboxMapper)
        : base(configuration, dbSession)
    {
        _outboxMapper = outboxMapper ?? throw new ArgumentNullException(nameof(outboxMapper));
    }

    #endregion

    #region Public methods

    /// <inheritdoc />
    public async Task SaveAsync<TContent>(Message<TContent> message)
    {
        OutboxMessage outboxMessage = _outboxMapper.Map(message);

        // Dapper vanilla.
        string sql = @"INSERT INTO [dbo].[OutboxMessages]
                           ([MessageId]
                           ,[CorrelationId]
                           ,[CausationId]
                           ,[CreatedOnUtc]
                           ,[ContentId]
                           ,[Host]
                           ,[Version]
                           ,[SavedOn]
                           ,[Content]
                           ,[ContentType]
                           ,[Error]
                           ,[Retries])
                     VALUES
                           (@messageId
                           ,@correlationId
                           ,@causationId
                           ,@CreatedOnUtc
                           ,@ContentId
                           ,@host
                           ,@version
                           ,@savedOn
                           ,@content
                           ,@contentType
                           ,@error
                           ,@retries)";

        await _dbSession.Connection!.ExecuteAsync(
            sql,
            new
            {
                messageId = outboxMessage.MessageId,
                correlationId = outboxMessage.CorrelationId,
                causationId = outboxMessage.CausationId,
                createdOnUtc = outboxMessage.CreatedOnUtc,
                contentId = outboxMessage.ContentId,
                host = outboxMessage.Host,
                version = outboxMessage.Version,
                savedOn = outboxMessage.SavedOn,
                content = outboxMessage.Content,
                contentType = outboxMessage.ContentType,
                error = outboxMessage.Error,
                retries = outboxMessage.Retries,
            },
            _dbSession.Transaction);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<OutboxMessage>?> GetAllAsync()
    {
        const string sql = @"
            SELECT OM.* 
            FROM OutboxMessages OM with (NOLOCK)
            ORDER BY SavedOn";

        IEnumerable<OutboxMessage>? outboxMessages = await _dbSession.Connection!.QueryAsync<OutboxMessage>(sql);

        return outboxMessages;
    }

    /// <inheritdoc />
    public async Task PublishedAsync(Guid messageId)
    {
        const string sql = "DELETE FROM OutboxMessages WHERE MessageId = @messageId;";

        ////int rowsAffected = await _dbSession.Connection.ExecuteAsync(sql, new { messageId });
        await _dbSession.Connection!.ExecuteAsync(sql, new { messageId });
    }

    /// <inheritdoc />
    public async Task FailedAsync(Guid messageId, string error)
    {
        string sql = @"
            UPDATE OutboxMessages
                SET Retries = Retries + 1, 
                    Error = CONCAT(Error, @error, '¬')
            WHERE MessageId = @messageId;";

        ////int rowsAffected = await _dbSession.Connection.ExecuteAsync(sql, new { messageId, error });
        await _dbSession.Connection!.ExecuteAsync(sql, new { messageId, error });
    }

    #endregion
}
