#region Usings

using Microsoft.Extensions.Options;
using Records.Shared.Configuration;
using Records.Shared.Infra.Idempotence.Abstractions;
using StackExchange.Redis;

#endregion

namespace Records.Shared.Infra.Idempotence.Redis;

/// <summary>
/// Represent a class to manage and check idempotentcy in messages (with Redis).
/// </summary>
public class IdempotentMessageService : IIdempotentMessageService
{
    #region Declarations

    private const string KeySuffix = "idempotency";

    private readonly IConnectionMultiplexer _connectionMultiplexer;

    private readonly RedisSettings _redisSettings;

    #endregion

    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="IdempotentMessageService"/> class.
    /// </summary>
    /// <param name="connectionMultiplexer">Represents the abstract multiplexer API (StackExchange).</param>
    /// <param name="redisSettings">The settings for Redis.</param>
    public IdempotentMessageService(IConnectionMultiplexer connectionMultiplexer, IOptions<RedisSettings> redisSettings)
    {
        ArgumentNullException.ThrowIfNull(redisSettings);

        _connectionMultiplexer = connectionMultiplexer;
        _redisSettings = redisSettings.Value;
    }

    #endregion

    #region Public methods

    /// <inheritdoc />
    public async Task<bool> HasBeenProcessed(string messageId, string messageTypeName)
    {
        IDatabase redisdb = _connectionMultiplexer.GetDatabase();

        return await redisdb.KeyExistsAsync($"{_redisSettings.ProductName}-{KeySuffix}:{messageTypeName}:{messageId}");
    }

    /// <inheritdoc />
    public void Save(string messageId, string messageTypeName)
    {
        IDatabase redisdb = _connectionMultiplexer.GetDatabase();

        // key: {productName}:{messageTypeName}:{messageId}
        // value: utc timestamp in epoch format.
        // expires: TtlIdempotentEvents setting.
        //
        // *To keep it simple and fast just store the timestamp without any json serialization.
        redisdb.StringSet(
            $"{_redisSettings.ProductName}-{KeySuffix}:{messageTypeName}:{messageId}",
            DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString(),
            TimeSpan.FromSeconds(_redisSettings.TtlIdempotentEvents));
    }

    /// <inheritdoc />
    public async Task SaveAsync(string messageId, string messageTypeName)
    {
        IDatabase redisdb = _connectionMultiplexer.GetDatabase();

        // key: {productName}:{messageTypeName}:{messageId}
        // value*: utc timestamp in epoch format.
        // expires: TtlIdempotentEvents setting.
        //
        // *To keep it simple and fast just store the timestamp without any json serialization.
        await redisdb.StringSetAsync(
            $"{_redisSettings.ProductName}-{KeySuffix}:{messageTypeName}:{messageId}",
            DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString(),
            TimeSpan.FromSeconds(_redisSettings.TtlIdempotentEvents));
    }

    #endregion
}
