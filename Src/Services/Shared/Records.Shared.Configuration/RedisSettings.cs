namespace Records.Shared.Configuration;

/// <summary>
/// Represents the settings that will be mapped from the Redis key in the appsettings.json file.
/// </summary>
public class RedisSettings
{
    #region Declarations

    /// <summary>The key to map from the appsettings.json file.</summary>
    public const string SettingsKey = "Redis"; // Without "...Settings" suffix.

    #endregion

    #region Properties

    /// <summary>Redis host.</summary>
    public string Host { get; init; } = string.Empty;

    /// <summary>Redis username.</summary>
    public string Username { get; init; } = string.Empty;

    /// <summary>Redis password.</summary>
    public string Password { get; init; } = string.Empty;

    /// <summary>To prefix all the keys.</summary>
    public string ProductName { get; init; } = string.Empty;

    /// <summary>Time to live (in seconds) for the idempotency keys.</summary>
    public int TtlIdempotentEvents { get; init; } = -1;

    #endregion
}
