#region Usings

using System.Reflection;
using FluentValidation;
using Records.Countries.Infra.Client;
using Records.Persons.Api.V1.HealthCheck;
using Records.Persons.Application.Persons.Commands.CreatePerson;
using Records.Persons.Application.Persons.Consumers;
using Records.Persons.Configuration.DependencyInjection;
using Records.Persons.Domain.Countries.Services;
using Records.Persons.Infra.Persistence.Sql.Persons.Mappers;
using Records.Persons.Infra.Persistence.Sql.Persons.Repositories;
using Records.Persons.Infra.Reads.sql.Persons.GetPersons;
using Records.Shared.Application.DependencyInjection;
using Records.Shared.Infra.Http.DependencyInjection;
using Records.Shared.Infra.Mappings.DependencyInjection;
using Records.Shared.Infra.MessageBroker.MassTransit.DI;
using Records.Shared.Infra.Outbox.Sql.Mappers;
using Records.Shared.Infra.Outbox.Sql.Repositories;
using Records.Shared.Infra.Persistence.DependencyInjection;
using Records.Shared.Infra.Reads.DependencyInjection;
using Records.Shared.Infra.Redis.DependencyInjection;
using Records.Shared.Infra.Serilog.DependencyInjection;
using Records.Shared.Infra.Swagger.DependencyInjection;

#endregion

namespace Records.Persons.Api.V1;

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

        // Configuration.
        builder.Services.AddConfiguration(builder.Configuration);

        // Persistence.
        builder.Services.AddPersistence(
            typeof(PersonRepository).Assembly,
            typeof(OutboxRepository).Assembly);

        // Application.
        builder.Services.AddApplication(
            typeof(CreatePersonCommand).Assembly,
            typeof(CountryService).Assembly);

        // Redis.
        builder.Services.AddRedis(builder.Configuration);

        // MessageBrokerWithMassTransitForApi.
        builder.Services
            .AddMessageBrokerWithMassTransitForApi(
                builder.Configuration,
                typeof(PersonCreatedConsumer).Assembly);

        // Mapster.
        builder.Services.AddMapster(
            Assembly.GetExecutingAssembly(),
            ////typeof(CreatePersonMapper).Assembly,
            typeof(PersonMapper).Assembly,
            typeof(OutboxMapper).Assembly);

        // Mappers.
        builder.Services.AddMappers(
            Assembly.GetExecutingAssembly(),
            ////typeof(CreatePersonMapper).Assembly,
            typeof(PersonMapper).Assembly,
            typeof(OutboxMapper).Assembly);

        // Read repositories.
        builder.Services.AddReadsRepositories(
            typeof(GetPersonsRepository).Assembly);

        // HealthChecks.
        builder.Services.AddHealthChecksCustom(builder.Configuration);

        // Api.
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerCustom("v1", "Records.Persons API", "Persons microservice.");

        // Middlewares.
        builder.Services.AddGlobalExceptionHandler();

        // FluentValidation validators.
        builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        //// FIX: FluentValidation customization: 'First name' is invalid, to 'FirstName' is invalid.
        ValidatorOptions.Global.DisplayNameResolver = (_, member, _) =>
        {
            return member?.Name;
        };

        // Configures and registers Serilog as logger.
        ////builder.Host.UseSerilogCustom();
        builder.UseSerilogCustom();

        // Adds a typed httpClient (CountriesClient) with some policies to the container to be
        // able to communicate via API to the Countries microservice.
        builder.Services.AddCountriesHttpClinet(builder.Configuration);

        // Builds the app.
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
