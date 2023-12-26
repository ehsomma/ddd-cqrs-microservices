#region Usings

using Microsoft.Extensions.Options;
using Records.Shared.Configuration;
using Records.Shared.Infra.Idempotence.Abstractions;
using StackExchange.Redis;

#endregion

namespace Records.Shared.Infra.Idempotence.Redis;

/// <summary>
/// Represent a class to manage and check obsoletes messages (with Redis).
/// </summary>
public class ObsoleteMessageService : IObsoleteMessageService
{
    #region Declarations

    private const string KeySuffix = "obsoletesPrevention";

    private readonly IConnectionMultiplexer _connectionMultiplexer;

    private readonly RedisSettings _redisSettings;

    #endregion

    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="ObsoleteMessageService"/> class.
    /// </summary>
    /// <param name="connectionMultiplexer">Represents the abstract multiplexer API (StackExchange).</param>
    /// <param name="redisSettings">The settings for Redis.</param>
    public ObsoleteMessageService(IConnectionMultiplexer connectionMultiplexer, IOptions<RedisSettings> redisSettings)
    {
        ArgumentNullException.ThrowIfNull(redisSettings);

        _connectionMultiplexer = connectionMultiplexer;
        _redisSettings = redisSettings.Value;
    }

    #endregion

    #region Public methods

    /// <inheritdoc />
    public async Task<bool> IsNotObsolete(string messageTypeName, DateTime createdOnUtc, string contentId)
    {
        /*
        How to avoid "obsolete updates".

        Case example:
            Microservice 1
            MSG_1 with entity #1 (updated "born" 10:00) => Publish Fail => reintent in 5'' => Publish ok
            MSG_2 with entity #1 (updated "grew" 10:05) => Publish ok

            Microservice 2
            Consume MSG_2 ok, entity #1 update to "grew" (u10:05)
            Consume MSG_1 ok, entity #1 update to "born" (u10:01) [!] Obsolet!

        Option 1 (este ejemplo):
        Guardar en cache la última Message.Metadata.CreatedOn para cada entidad/agregado
        y al consumir el mensaje verificar que el nuevo Message.Metadata.CreatedOn
        no sea < que el Message.Metadata.CreatedOn guardado.

        Option 2:
        Otra forma es crear un campo _SourceMessageCreaterdOn en la tabla de entidad/agregado
        y, desde el repo, antes de actualizar, obtener ese campo y verificar que el nuevo
        Message.Metadata.CreatedOn no sea < que el campo _SourceMessageCreaterdOn.
        */

        bool ret = true;

        IDatabase redisdb = _connectionMultiplexer.GetDatabase();

        RedisValue value = await redisdb.StringGetAsync($"{_redisSettings.ProductName}-{KeySuffix}:{messageTypeName}:{contentId}");

        if (value.HasValue
            && value.TryParse(out long lastProcessedMessageCreatedOn))
        {
            long currentMessageCreateOn = new DateTimeOffset(createdOnUtc).ToUnixTimeMilliseconds();

            // Si la fecha de creación del mensaje actual a procesar, es menor a la fecha de
            // creación del último mensaje procesado, este mensaje es obsoleto ya que por algún
            // motivo quedó en espera y otro mensaje posterior para la misma entidad/agregado ya
            // fue procesado.
            if (currentMessageCreateOn < lastProcessedMessageCreatedOn)
            {
                ret = false;
            }
        }

        return ret;
    }

    /// <inheritdoc />
    public void Save(string messageTypeName, DateTime createdOnUtc, string contentId)
    {
        IDatabase redisdb = _connectionMultiplexer.GetDatabase();

        // key: {productName}:{messageTypeName}:{contentId}
        // value: utc timestamp in epoch format of the message creation of the last processed message.
        // expires: TtlIdempotentEvents setting.
        //
        // *To keep it simple and fast just store the timestamp without any json serialization.
        redisdb.StringSet(
            $"{_redisSettings.ProductName}-{KeySuffix}:{messageTypeName}:{contentId}",
            new DateTimeOffset(createdOnUtc).ToUnixTimeMilliseconds().ToString(),
            TimeSpan.FromSeconds(_redisSettings.TtlIdempotentEvents));
    }

    /// <inheritdoc />
    public async Task SaveAsync(string messageTypeName, DateTime createdOnUtc, string contentId)
    {
        IDatabase redisdb = _connectionMultiplexer.GetDatabase();

        // key: {productName}:{messageTypeName}:{contentId}
        // value: utc timestamp in epoch format of the message creation of the last processed message.
        // expires: TtlIdempotentEvents setting.
        //
        // *To keep it simple and fast just store the timestamp without any json serialization.
        await redisdb.StringSetAsync(
            $"{_redisSettings.ProductName}-{KeySuffix}:{messageTypeName}:{contentId}",
            new DateTimeOffset(createdOnUtc).ToUnixTimeMilliseconds().ToString(),
            TimeSpan.FromSeconds(_redisSettings.TtlIdempotentEvents));
    }

    #endregion
}
