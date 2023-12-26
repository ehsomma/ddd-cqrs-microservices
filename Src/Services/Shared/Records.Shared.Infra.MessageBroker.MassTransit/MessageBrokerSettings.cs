namespace Records.Shared.Infra.MessageBroker.MassTransit;

/// <summary>
/// Represents the settings that will be mapped from the MessageBroker key in the appsettings.json file.
/// </summary>
public class MessageBrokerSettings
{
    #region Declarations

    /// <summary>The key to map from the appsettings.json file.</summary>
    public const string SettingsKey = "MessageBroker"; // Without "...Settings" suffix.

    #endregion

    #region Properties

    /// <summary>The host name.</summary>
    public string Host { get; init; } = string.Empty;

    /// <summary>The username.</summary>
    public string Username { get; init; } = string.Empty;

    /// <summary>The password.</summary>
    public string Password { get; init; } = string.Empty;

    /// <summary>If the RetryPolicy must be enabled (default true).</summary>
    public bool DefaultRetryPolicyEnable { get; init; } = true;

    /// <summary>The RetryPolicy maximum retries (default 3).</summary>
    public int DefaultRetryPolicyMaxRetries { get; init; } = 3;

    /// <summary>The RetryPolicy interval in miliseconds (default 5000).</summary>
    public int DefaultRetryPolicyInterval { get; init; } = 5000;

    #endregion
}
