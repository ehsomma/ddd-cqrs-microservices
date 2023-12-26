#region Usings

using Microsoft.Extensions.DependencyInjection;
using Records.Shared.Projection.Abstractions;
using System.Reflection;

#endregion

namespace Records.Shared.Infra.Projection.DependencyInjection;

/// <summary>
/// Extensions methods for dependency injection.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds the projectors to the container.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="assemblies">The assemblies to should be scanned.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddProjectors(this IServiceCollection services, params Assembly[] assemblies)
    {
        // UnitOfWork.
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IDbSession, DbSessionProjection>();

        services.Scan(selector =>
        {
            selector.FromAssemblies(assemblies)
                .AddClasses(classes =>
                {
                    classes.Where(type => type.Name.EndsWith("Projector"));
                })
                .AsImplementedInterfaces()
                .WithTransientLifetime();
        });

        return services;
    }
}
