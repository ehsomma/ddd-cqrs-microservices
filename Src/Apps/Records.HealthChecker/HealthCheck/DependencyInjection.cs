namespace Records.HealthChecker.HealthCheck;

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
        ArgumentNullException.ThrowIfNull(services);

        // Requiere:
        // • AspNetCore.HealthChecks.UI
        // • AspNetCore.HealthChecks.UI.InMemory.Storage
        services.AddHealthChecksUI(config =>
        {
            config.SetEvaluationTimeInSeconds(5); // [!]IMPORTANT: Change to 60 in production.
        }).AddInMemoryStorage();

        return services;
    }

    /// <summary>
    /// Adds a middleware that provides health check status.
    /// </summary>
    /// <param name="app">The <see cref="IApplicationBuilder"/> to add the middleware to.</param>
    /// <returns>The same application builder.</returns>
    public static IApplicationBuilder UseHealthChecksCustom(this WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);

        // FIX: UseHealthChecksUI not showing "Pooling interval" secs in the UI. Use MapHealthChecksUI!
        // [!] Require appsettings.json configuration.
        // Require:
        // • AspNetCore.HealthChecks.UI
        /*
        app.UseHealthChecksUI(config =>
        {
            config.UIPath = "/health-ui";
            config.AddCustomStylesheet("HealthCheck\\health-check.css");
            config.PageTitle = "Records Health Checker";
        });
        */

        // [!] Require appsettings.json configuration.
        // Require:
        // • AspNetCore.HealthChecks.UI
        app.MapHealthChecksUI(config =>
        {
            config.UIPath = "/health-ui";
            config.PageTitle = "Records Health Checker";
            ////config.AddCustomStylesheet("HealthCheck\\health-check.css"); // Custom .css.
        });

        return app;
    }

    #endregion
}
