#region Usings

using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

#endregion

namespace Records.Shared.Infra.Reads.DependencyInjection;

/// <summary>
/// Extensions methods for dependency injection.
/// </summary>
public static class DependencyInjection
{
    #region Public methods

    /// <summary>
    /// Registers all the Repositories.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="assemblies">The assemblies to should be scanned.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddReadsRepositories(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.Scan(selector =>
        {
            selector.FromAssemblies(assemblies)
                .AddClasses(classes =>
                {
                    classes.Where(type => type.Name.EndsWith("Repository"));
                })
                .AsImplementedInterfaces()
                .WithTransientLifetime();
        });

        return services;
    }

    #endregion
}
