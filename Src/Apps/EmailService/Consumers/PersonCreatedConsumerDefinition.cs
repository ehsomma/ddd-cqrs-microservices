using MassTransit;

namespace EmailService.Consumers;

/// <summary>
/// Represents the policies configuration of <see cref="PersonCreatedConsumer"/>.
/// </summary>
public class PersonCreatedConsumerDefinition : ConsumerDefinition<PersonCreatedConsumer>
{
    #region Methods

    /// <summary>
    /// Configures the policies for <see cref="PersonCreatedConsumer"/>.
    /// </summary>
    /// <param name="endpointConfigurator">An <see cref="IReceiveEndpointConfigurator"/>.</param>
    /// <param name="cfg">An <see cref="IConsumerConfigurator"/>.</param>
    /// <param name="context">The context.</param>
    protected override void ConfigureConsumer(
        IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<PersonCreatedConsumer> cfg,
        IRegistrationContext context)
    {
        // This policy override the global policy.
        cfg.UseMessageRetry(r => r.Intervals(5000, 10000, 60000));
    }

    #endregion
}
