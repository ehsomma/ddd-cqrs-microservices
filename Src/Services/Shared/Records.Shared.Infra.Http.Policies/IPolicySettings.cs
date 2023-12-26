namespace Records.Shared.Infra.Http.Policies;

/// <summary>
/// Defines the settings that will be mapped from the CountriesClientPolicies key in the
/// appsettings.json file.
/// </summary>
public interface IPolicySettings
{
    #region Properties

    /// <summary>HandledEventsAllowedBeforeBreaking (CircuitBreaker policy).</summary>
    public int HandledEventsAllowedBeforeBreaking { get; init; }

    /// <summary>DurationOfBreak in milliseconds (CircuitBreaker policy).</summary>
    public double DurationOfBreak { get; init; }

    /// <summary>RetryCount (Retry policy).</summary>
    public int RetryCount { get; init; }

    /// <summary>TimeToRetry in milliseconds (Retry policy).</summary>
    public double TimeToRetry { get; init; }

    /// <summary>TimeoutTime in milliseconds (Timeout policy).</summary>
    public double TimeoutTime { get; init; }

    #endregion
}
