#region Usings

using Microsoft.Extensions.DependencyInjection;
using Records.Shared.Infra.Persistence.Abstractions;
using System.Reflection;

#endregion

namespace Records.Shared.Infra.Persistence.DependencyInjection;

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
    /// <param name="assemblies">Assemblies to scan.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddPersistence(this IServiceCollection services, params Assembly[] assemblies)
    {
        // UnitOfWork.
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IDbSession, DbSession>();

        // Repositories.
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
