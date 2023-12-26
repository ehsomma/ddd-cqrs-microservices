#region Usings

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DtoShared = Records.Persons.Dtos.Shared;

#endregion

namespace Records.Persons.Configuration.DependencyInjection;

/// <summary>
/// Extensions methods for dependency injection.
/// </summary>
public static class DependencyInjection
{
    #region Public methods

    /// <summary>
    /// Registers the necessary configurations with the DI framework.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">Represents a set of key/value application configuration properties.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        /*
        Nuget packages required to use DI:
        • Microsoft.Extensions.DependencyInjection.Abstractions // DI

        Nuget packages required to use IConfiguration, .Configure, .GetSection and .Get:
        • Microsoft.Extensions.Configuration.Abstractions // IConfiguration
        • Microsoft.Extensions.Options.ConfigurationExtensions // .Configure
        • Microsoft.Extensions.Configuration.Binder // .Get
        */

        // To inject IOptions<AppSettings> via constructor.
        // To inject IOptionsSnapshot<AppSettings> via constructor.
        // To inject IOptionsMonitor<AppSettings> via constructor.
        services.Configure<DtoShared.PersonsSettings>(configuration.GetSection(DtoShared.PersonsSettings.SettingsKey));

        // AppSettingsService (singleton).
        ////services.AddSingleton<IAppSettingsService, AppSettingsService>();

        // To use here.
        ////AppSettings settings = configuration.GetSection(AppSettings.SettingsKey).Get<AppSettings>();

        return services;
    }

    #endregion
}
