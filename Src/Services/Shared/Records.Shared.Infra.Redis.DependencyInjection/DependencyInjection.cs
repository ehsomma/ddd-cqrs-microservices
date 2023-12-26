#region Usings

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Records.Shared.Configuration;
using StackExchange.Redis;
using System;

#endregion

namespace Records.Shared.Infra.Redis.DependencyInjection;

/// <summary>
/// Extensions methods for dependency injection.
/// </summary>
public static class DependencyInjection
{
    #region Public methods

    /// <summary>
    /// Registers the necessary services with the DI framework.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">Represents a set of key/value application configuration properties.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        // To inject IOptions<AppSettings> by constructor in controllers etc.
        services.Configure<RedisSettings>(configuration.GetSection(RedisSettings.SettingsKey));

        RedisSettings redisSettings = configuration.GetSectionOrThrow<RedisSettings>(RedisSettings.SettingsKey);

        services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            return ConnectionMultiplexer.Connect(redisSettings.Host, opt =>
            {
                opt.Password = redisSettings.Password;
            });
        });

        return services;
    }

    #endregion
}
