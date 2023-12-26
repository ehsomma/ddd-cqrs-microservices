#region Usings

using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Records.Shared.Configuration;
using Records.Shared.Infra.MessageBroker.MassTransit;

#endregion

namespace Records.Persons.Api.V1.HealthCheck;

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
    /// <param name="configuration">Represents a set of key/value application configuration properties.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddHealthChecksCustom(this IServiceCollection services, IConfiguration configuration)
    {
        /*
        HealthCheckResult:
        • Healthy – our application is healthy and in a normal, working state.
        • Unhealthy – our application is unhealthy and is offline or an unhandled exception was thrown while executing the check.
        • Degraded – our application is still running, but not responding within an expected timeframe.
        */

        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        SqlServerSettings databaseSettings = configuration.GetSectionOrThrow<SqlServerSettings>(SqlServerSettings.SettingsKey);
        RedisSettings redisSettings = configuration.GetSectionOrThrow<RedisSettings>(RedisSettings.SettingsKey);
        MessageBrokerSettings rabbitSettings = configuration.GetSectionOrThrow<MessageBrokerSettings>(MessageBrokerSettings.SettingsKey);

        // E.g: "Server=localhost;Database=RecordsPersons_Source;User Id=sa;Password=xxxxx;App=Records;MultipleActiveResultSets=True".
        // FIX: Needs to add "Encrypt=False" to the end to avoid error:
        // "A connection was successfully established with the server, but then an error occurred during the login process. (provider: SSL Provider, error: 0 - La cadena de certificación fue emitida por una entidad en la que no se confía.)".
        // Source: https://community.fabric.microsoft.com/t5/Report-Server/Microsoft-SQL-A-connection-was-successfully-established-with-the/m-p/2323459.
        string sqlServerSourceConnectionString = $"{databaseSettings.SourceConnectionString};Encrypt=False";
        string sqlServerProjectionConnectionString = $"{databaseSettings.ProjectionConnectionString};Encrypt=False";

        // E.g: "amqp://guest:guest@localhost:5672".
        string rabbitConnectionString = $"amqp://{rabbitSettings.Username}:{rabbitSettings.Password}@{rabbitSettings.Host}";

        // E.g: "localhost:6379,password=xxxxx".
        string redisConnectionString = $"{redisSettings.Host},password={redisSettings.Password}";

        // Requiere:
        // • Microsoft.Bcl.AsyncInterfaces
        // • AspNetCore.HealthChecks.UI.Client
        // • AspNetCore.HealthChecks.Xxxxxx
        services.AddHealthChecks()
            .AddCheck("Persons.Api", () => HealthCheckResult.Healthy("Ready"), new[] { "api" })
            .AddSqlServer(sqlServerSourceConnectionString, name: "SqlServer Source", tags: new[] { "database" })
            .AddSqlServer(sqlServerProjectionConnectionString, name: "SqlServer Projection", tags: new[] { "database" })
            .AddRabbitMQ(rabbitConnectionString: rabbitConnectionString)
            .AddRedis(redisConnectionString);

        // Custom health check.
        services.AddHealthChecks()
            .AddCheck<CustomHealthCheck>(nameof(CustomHealthCheck), tags: new[] { "custom", "demo" });

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

        // Muestra solo healthy.
        ////app.MapHealthChecks("/health");

        // Muestra json con todos los health checks.
        // Requiere:
        // • AspNetCore.HealthChecks.UI.Client
        app.UseHealthChecks("/health", new HealthCheckOptions()
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
        });

        // Muestra json con los health checks con tag "database".
        app.UseHealthChecks("/health/databases", new HealthCheckOptions()
        {
            Predicate = reg => reg.Tags.Contains("database"),
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
        });

        return app;
    }

    #endregion
}
