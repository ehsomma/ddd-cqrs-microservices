#region Usings

using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

#endregion

namespace Records.Countries.BackgroundTasks.HealthCheck;

/// <summary>
/// Extensions methods for dependency injection.
/// </summary>
public static class DependencyInjection
{
    #region Public methods

    /// <summary>
    /// Adds HealthCheckService to the container.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddHealthChecksCustom(this IServiceCollection services)
    {
        // Adds HealthCheckService to the container.
        // Requiere:
        // • Microsoft.Bcl.AsyncInterfaces
        // • AspNetCore.HealthChecks.UI.Client
        services.AddHealthChecks()
            .AddCheck("Countries.BackgroundTasks", () => HealthCheckResult.Healthy("Ready"), new[] { "service" });

        return services;
    }

    /// <summary>
    /// Adds a middleware that provides health check status.
    /// </summary>
    /// <param name="app">The <see cref="IApplicationBuilder"/> to add the middleware to.</param>
    /// <returns>The same application builder.</returns>
    public static IApplicationBuilder UseHealthChecksCustom(this IApplicationBuilder app)
    {
        // Muestra json con todos los health checks.
        // Requiere:
        // • AspNetCore.HealthChecks.UI.Client
        app.UseHealthChecks("/health", new HealthCheckOptions()
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
        });

        return app;
    }

    #endregion
}
