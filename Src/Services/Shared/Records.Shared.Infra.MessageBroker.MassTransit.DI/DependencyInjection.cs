#region Usings

using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Records.Shared.Infra.Idempotence.Abstractions;
using Records.Shared.Infra.Idempotence.Redis;
using Records.Shared.Infra.MessageBroker.Abstractions;
using System;
using System.Reflection;

#endregion

namespace Records.Shared.Infra.MessageBroker.MassTransit.DI;

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
    /// <param name="consumersAssembly">The assembly to scan for consumers.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddMessageBrokerWithMassTransitForApi(
        this IServiceCollection services,
        IConfiguration configuration,
        Assembly consumersAssembly)
    {
        return AddMessageBrokerWithMassTransit(services, configuration, consumersAssembly, true);
    }

    /// <summary>
    /// Registers the necessary services with the DI framework.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">Represents a set of key/value application configuration properties.</param>
    /// <param name="consumersAssembly">The assembly to scan for consumers.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddMessageBrokerWithMassTransitForWorker(
        this IServiceCollection services,
        IConfiguration configuration,
        Assembly consumersAssembly)
    {
        return AddMessageBrokerWithMassTransit(services, configuration, consumersAssembly, false);
    }

    /// <summary>
    /// Registers the necessary services with the DI framework.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">Represents a set of key/value application configuration properties.</param>
    /// <param name="consumersAssembly">The assembly to scan for consumers.</param>
    /// <param name="forApi">True, registers the EventBus for APIs. Otherwise, registers for Workers.</param>
    /// <returns>The same service collection.</returns>
    private static IServiceCollection AddMessageBrokerWithMassTransit(
        this IServiceCollection services,
        IConfiguration configuration,
        Assembly consumersAssembly,
        bool forApi)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        // Idempotency service for messages.
        services.AddSingleton<IIdempotentMessageService, IdempotentMessageService>();
        services.AddSingleton<IObsoleteMessageService, ObsoleteMessageService>();

        // MessageBrokerSettings.
        MessageBrokerSettings? messageBrokerSettings = configuration.GetSection(MessageBrokerSettings.SettingsKey).Get<MessageBrokerSettings>()
                                                       ?? throw new Exception("MessageBroker settings not found in appsetings");
        services.AddSingleton(sp => sp.GetRequiredService<IOptions<MessageBrokerSettings>>().Value);

        // IEventBus.
        // View: EventBusForApi vs EventBusForWorker.
        if (forApi)
        {
            // For APIs (uses IPublishEndpoint and ISendEndpointProvider).
            services.AddTransient<IEventBus, EventBusForApi>();
        }
        else
        {
            // For Workers (uses IBus).
            services.AddTransient<IEventBus, EventBusForWorker>();
        }

        // MassTransit + consumers.
        services.AddMassTransit(busCfg =>
        {
            busCfg.SetKebabCaseEndpointNameFormatter();

            ////var entryAssembly = Assembly.GetEntryAssembly(); // NOTE: It only works if its are in this assembly.
            ////Assembly entryAssembly = Assembly.GetAssembly(PersonCreatedIntegrationEventConsumer);
            Assembly entryAssembly = consumersAssembly;
            busCfg.AddConsumers(entryAssembly);

            // Used in cfg.ConfigureEndpoints bellow.
            ////busCfg.SetEndpointNameFormatter(new CustomEndpointNameFormatter("my-custom-prefix-{0}"));
            ////busCfg.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("test", true));

            busCfg.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(messageBrokerSettings.Host, h =>
                {
                    h.Username(messageBrokerSettings.Username);
                    h.Password(messageBrokerSettings.Password);
                });

                // Default retry policy (3 time with 5sec. of interval) for all consumers.
                // If you want a custom policy for a specific consumer, create a ConsumerDefinition,
                // that ConsumerDefinition will override this global policy.
                if (messageBrokerSettings.DefaultRetryPolicyEnable)
                {
                    cfg.UseMessageRetry(r => r.Interval(
                        messageBrokerSettings.DefaultRetryPolicyMaxRetries,
                        messageBrokerSettings.DefaultRetryPolicyInterval));
                }

                // (Obsolet) Now uses ConsumerMetadataSupport class. See IdempotentConsumerFilter
                // obsolete comment for more details.
                // Idempotency filter.
                ////cfg.UseConsumeFilter(typeof(IdempotentConsumerFilter<>), context);

                // Adds the global exception filter.
                cfg.UseConsumeFilter(typeof(GlobalExceptionFilter<>), context);

                // Configure the endpoints for all defined consumer, saga, and activity types.
                // NOTE: It includes the namespace because we consume the same event y several projects
                // and we use the same consumer name, so this way will prefix the namespace to the
                // queues names.
                cfg.ConfigureEndpoints(context, new KebabCaseEndpointNameFormatter(true));
            });
        });

        return services;
    }

    #endregion
}
