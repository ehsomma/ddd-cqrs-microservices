using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Records.Persons.Api.V1.HealthCheck;

/// <inheritdoc />
public class CustomHealthCheck : IHealthCheck
{
    #region Declarations

    ////private readonly Random _random = new ();

    #endregion

    #region Private methods

    /// <inheritdoc />
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        // NOTE: This health check do nothing, it is just for demo purpose.
        ////int responseTime = _random.Next(1, 300);

        HealthCheckResult ret;

        // NOTE: Just for demo, we response always healthy.
        ret = HealthCheckResult.Healthy("Healthy result from CustomHealthCheck (demo)");

        /*
        switch (responseTime)
        {
            case < 100:
                ret = HealthCheckResult.Healthy("Healthy result from CustomHealthCheck (demo)");
                break;

            case < 200:
                ret = HealthCheckResult.Degraded("Degraded result from CustomHealthCheck (demo)");
                break;

            default:
                ret = HealthCheckResult.Unhealthy("Unhealthy result from CustomHealthCheck (demo)");
                break;
        }
        */

        return Task.FromResult(ret);
    }

    #endregion
}
