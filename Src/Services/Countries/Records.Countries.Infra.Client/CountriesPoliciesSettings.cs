using Records.Shared.Infra.Http.Policies;

namespace Records.Countries.Infra.Client;

/// <summary>
/// Represents the settings that will be mapped from the CountriesClientPolicies key in the
/// appsettings.json file.
/// </summary>
public class CountriesPoliciesSettings : IPolicySettings
{
    #region Declarations

    /// <summary>The key to map from the appsettings.json file.</summary>
    public const string SettingsKey = "CountriesClientPolicies"; // Without "...Settings" suffix.

    #endregion

    #region Properties

    /// <inheritdoc />
    public int HandledEventsAllowedBeforeBreaking { get; init; }

    /// <inheritdoc />
    public double DurationOfBreak { get; init; }

    /// <inheritdoc />
    public int RetryCount { get; init; }

    /// <inheritdoc />
    public double TimeToRetry { get; init; }

    /// <inheritdoc />
    public double TimeoutTime { get; init; }

    #endregion
}
