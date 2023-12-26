#region Usings

using FluentValidation;
using Records.Countries.Api.V1.HealthCheck;
using Records.Countries.Application.Countries.Commands.CreateCountry;
using Records.Countries.Application.Countries.Consumers;
using Records.Countries.Infra.Persistence.Sql.Countries.Repositories;
using Records.Shared.Application.DependencyInjection;
using Records.Shared.Infra.Http.DependencyInjection;
using Records.Shared.Infra.Mappings.DependencyInjection;
using Records.Shared.Infra.MessageBroker.MassTransit.DI;
using Records.Shared.Infra.Outbox.Sql.Mappers;
using Records.Shared.Infra.Outbox.Sql.Repositories;
using Records.Shared.Infra.Persistence.DependencyInjection;
using Records.Shared.Infra.Redis.DependencyInjection;
using Records.Shared.Infra.Serilog.DependencyInjection;
using Records.Shared.Infra.Swagger.DependencyInjection;
using System.Reflection;

#endregion

namespace Records.Countries.Api.V1;

/// <summary>
/// Entry point of the application.
/// </summary>
public static class Program
{
    #region Public Methods

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
        ////var _enum = "hello"; // Sample to check CS0209, SA1312 and IDE0008.

        // Configuration.
        ////builder.Services.AddConfiguration(builder.Configuration);

        // Persistence.
        builder.Services.AddPersistence(
            typeof(CountryRepository).Assembly,
            typeof(OutboxRepository).Assembly);

        // Application.
        builder.Services.AddApplication(typeof(CreateCountryCommand).Assembly);

        // Redis.
        builder.Services.AddRedis(builder.Configuration);

        // MessageBrokerWithMassTransitForApi.
        builder.Services
            .AddMessageBrokerWithMassTransitForApi(
                builder.Configuration,
                typeof(CountryCreatedConsumer).Assembly);

        // Mapster.
        builder.Services.AddMapster(
            Assembly.GetExecutingAssembly(),
            ////typeof(CreateCountryMapper).Assembly,
            ////typeof(CountryMapper).Assembly,
            typeof(OutboxMapper).Assembly);

        // Mappers.
        builder.Services.AddMappers(
            Assembly.GetExecutingAssembly(),
            ////typeof(CreateCountryMapper).Assembly,
            ////typeof(CountryMapper).Assembly,
            typeof(OutboxMapper).Assembly);

        // HealthChecks.
        builder.Services.AddHealthChecksCustom(builder.Configuration);

        // Api.
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerCustom("v1", "Records.Countries API", "Countries microservice.");

        // Middlewares.
        builder.Services.AddGlobalExceptionHandler();

        // FluentValidation validators.
        builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        //// FIX: FluentValidation customization: 'First name' is invalid, to 'FirstName' is invalid.
        FluentValidation.ValidatorOptions.Global.DisplayNameResolver = (type, member, expression) =>
        {
            return member?.Name;
        };

        // Configures and registers Serilog as logger.
        ////builder.Host.UseSerilogCustom();
        builder.UseSerilogCustom();

        WebApplication app = builder.Build();

        // Swagger.
        app.UseSwaggerUICustom();

        // GlobalExceptionHandler.
        app.UseGlobalExceptionHandler();

        // HealthChecks.
        app.UseHealthChecksCustom();

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }

    #endregion
}
