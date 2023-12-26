#region Usings

using MediatR.NotificationPublishers;
using Microsoft.Extensions.DependencyInjection;
using Records.Shared.Domain.Services;
using System.Reflection;

#endregion

namespace Records.Shared.Application.DependencyInjection;

/// <summary>
/// Extensions methods for dependency injection.
/// </summary>
public static class DependencyInjection
{
    #region Public methods

    /// <summary>
    /// Adds MediatR and all domain services.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="assemblies">Assemblies to scan.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddApplication(this IServiceCollection services, params Assembly[] assemblies)
    {
        // MediatR 11.
        ////services.AddMediatR(Assembly.GetExecutingAssembly()); // If MediatR will be used in this assembly.
        ////services.AddMediatR(typeof(CreatePersonCommand).GetTypeInfo().Assembly);

        // MediatR 12.
        ////services.AddMediatR(config => config.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly())); // If MediatR will be used in this assembly.
        ////services.AddMediatR(config => config.RegisterServicesFromAssemblies(typeof(CreatePersonCommand).GetTypeInfo().Assembly));
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblies(assemblies);
            ////config.NotificationPublisher = new ForeachAwaitPublisher(); // Default. One by one with await.
            config.NotificationPublisher = new TaskWhenAllPublisher(); // In parallel (internally does a forEach and TaskWhenAll).
        });

        // Registers the domain services.
        services.Scan(selector =>
        {
            selector.FromAssemblies(assemblies)
                .AddClasses(classes =>
                {
                    classes
                        ////.Where(type => type.Name.EndsWith("Service"))
                        .AssignableTo<IDomainService>();
                })
                ////.AsSelf()
                .AsImplementedInterfaces()
                .WithTransientLifetime();
        });

        return services;
    }

    #endregion
}
