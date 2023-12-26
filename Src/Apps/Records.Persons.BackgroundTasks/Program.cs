#region Usings

using Records.Persons.BackgroundTasks.HealthCheck;
using Records.Persons.Configuration.DependencyInjection;
using Records.Persons.Infra.Projection.sql.Persons.Projectors;
using Records.Shared.Infra.Mappings.DependencyInjection;
using Records.Shared.Infra.MessageBroker.MassTransit.DI;
using Records.Shared.Infra.Outbox.Sql.Mappers;
using Records.Shared.Infra.Outbox.Sql.Repositories;
using Records.Shared.Infra.Persistence.DependencyInjection;
using Records.Shared.Infra.Projection.DependencyInjection;
using Records.Shared.Infra.Quartz.DependencyInjection;
using Records.Shared.Infra.Redis.DependencyInjection;
using Records.Shared.Infra.Serilog.DependencyInjection;
using Records.Shared.Infra.Swagger.DependencyInjection;
using System.Reflection;

#endregion

namespace Records.Persons.BackgroundTasks;

/// <summary>
/// Entry point of the application.
/// </summary>
public static class Program
{
    #region Public methods

    /// <summary>
    /// Creates an instance of the web application’s host. The host is responsible for bootstrapping
    /// the application and setting up the necessary services and middleware.
    ///
    /// It calls the Run method on the host, which starts the web server and listens for incoming
    /// HTTP requests.
    /// </summary>
    /// <param name="args">Arguments parameter that can be used to retrieve the arguments passed while running the application.</param>
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        // Configuration.
        builder.Services
            .AddConfiguration(builder.Configuration);

        // Redis.
        builder.Services.AddRedis(builder.Configuration);

        // MassTransit and Consumers.
        builder.Services
            .AddMessageBrokerWithMassTransitForApi(
                builder.Configuration,
                Assembly.GetExecutingAssembly());

        // Projector.
        builder.Services.AddProjectors(
            typeof(PersonCreatedProjector).Assembly);

        // Persistance.
        builder.Services.AddPersistence(
            typeof(OutboxRepository).Assembly);

        // Mappers.
        builder.Services.AddMappers(
            typeof(OutboxMapper).Assembly);

        // Quartz and jobs.
        ////builder.Services.AddQuartzCustom(builder.Configuration, JobsConfiguration.Configure); // Option 1: With an Action to register the Jobs.
        builder.Services.AddQuartzCustom(builder.Configuration); // Option 2: Iterating the assembly to search Jobs.

        // Configures and registers Serilog as logger.
        builder.UseSerilogCustom();

        // HealthChecks.
        builder.Services.AddHealthChecksCustom();

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerCustom("v1", "Records.Persons.BackgroundTasks", "BackgroundTasks/API for Persons microservice.");

        WebApplication app = builder.Build();

        // Swagger.
        app.UseSwaggerUICustom();

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        // HealthChecks.
        app.UseHealthChecksCustom();

        app.Run();
    }

    #endregion
}
