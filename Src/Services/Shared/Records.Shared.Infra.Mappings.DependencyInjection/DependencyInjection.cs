#region Usings

using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

#endregion

namespace Records.Shared.Infra.Mappings.DependencyInjection;

/// <summary>
/// Extensions methods for dependency injection.
/// </summary>
public static class DependencyInjection
{
    #region Public methods

    /// <summary>
    /// Registers the mappers.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="assemblies">Assemblies to scan.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddMappers(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.Scan(selector =>
        {
            selector.FromAssemblies(assemblies)
                .AddClasses(classes =>
                {
                    classes.Where(type => type.Name.EndsWith("Mapper"));
                })
                .AsImplementedInterfaces()
                .WithTransientLifetime();
        });

        return services;
    }

    /// <summary>
    /// Registers Mapster.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="assemblies">Assemblies to scan.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddMapster(this IServiceCollection services, params Assembly[] assemblies)
    {
        TypeAdapterConfig config = TypeAdapterConfig.GlobalSettings;
        config.Scan(assemblies);

        // Adds all IRegister scanned as singleton.
        services.AddSingleton(config);

        // Adds the mapper.
        services.AddScoped<IMapper, ServiceMapper>();

        return services;
    }

    #endregion
}
