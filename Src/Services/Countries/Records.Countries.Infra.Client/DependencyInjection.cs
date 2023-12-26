#region Usings

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Records.Shared.Infra.Http.Policies;

#endregion

namespace Records.Countries.Infra.Client;

/// <summary>
/// Extensions methods for dependency injection.
/// </summary>
public static class DependencyInjection
{
    #region Public methods

    /// <summary>
    /// Adds a typed httpClient (CountriesClient) with some policies to the container to be able to
    /// communicate via API to the Countries microservice.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddCountriesHttpClinet(this IServiceCollection services, IConfiguration configuration)
    {
        CountriesClientSettings clientSettings = configuration.GetSectionOrThrow<CountriesClientSettings>(CountriesClientSettings.SettingsKey);
        IPolicySettings policiesSettings = configuration.GetSectionOrThrow<CountriesPoliciesSettings>(CountriesPoliciesSettings.SettingsKey);

        // Adds a typed httpClient (CountriesClient) with some policies to the container.
        services.AddHttpClient<CountriesClient>(httpClient =>
        {
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            ////httpClient.DefaultRequestHeaders.Add("My-Header-Example1", clientSettings.HeaderExample1);
            httpClient.BaseAddress = new Uri(clientSettings.BaseUri); // "https://localhost:7197"
        })
        .SetHandlerLifetime(TimeSpan.FromMinutes(5)) // Sample: default lifetime is 2 minutes.
        ////.SetHandlerLifetime(Timeout.InfiniteTimeSpan)
        ////.ConfigurePrimaryHttpMessageHandler(() =>
        ////{
        ////    // NOTE: Si se usa typed clients, se inyecta como transient y si esto se usa
        ////    // en una clase singleton, hace lío. Para esto hay que usar ConfigurePrimaryHttpMessageHandler
        ////    // y SetHandlerLifetime como en este ejemplo.
        ////    // Source: https://youtu.be/g-JGay_lnWI.
        ////    // ReSharper disable once ConvertToLambdaExpression
        ////    return new SocketsHttpHandler
        ////    {
        ////        PooledConnectionIdleTimeout = TimeSpan.FromMinutes(5),
        ////    };
        ////})
        .AddPolicyHandler(PolicyProvider.GetCircuitBreakerPolicy(policiesSettings)) // Without provider and request parameters.
        ////.AddPolicyHandler(GetRetryPolicy(policiesSettings)) // Without provider and request parameters.
        ////.AddPolicyHandler(GetTimeOutPolicy(policiesSettings))
        ////.AddPolicyHandler((provider, request) => GetCircuitBreakerPolicy(policiesSettings, provider, request)) // It doesn't work with arguments.
        .AddPolicyHandler((provider, request) => PolicyProvider.GetRetryPolicy(policiesSettings, provider, request))
        .AddPolicyHandler((provider, request) => PolicyProvider.GetTimeOutPolicy(policiesSettings, provider, request));

        return services;
    }

    #endregion
}
