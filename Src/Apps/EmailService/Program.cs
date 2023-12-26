#region Usings

using EmailService.HealthCheck;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Records.Shared.Infra.MessageBroker.MassTransit.DI;
using Records.Shared.Infra.Redis.DependencyInjection;
using Records.Shared.Infra.Serilog.DependencyInjection;
using Records.Shared.Infra.Swagger.DependencyInjection;
using System.Reflection;

#endregion

namespace EmailService;

/// <summary>
/// Entry point of the application.
/// </summary>
public static class Program
{
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

        // Adds services to the container.

        // Redis.
        builder.Services.AddRedis(builder.Configuration);

        // MassTransit and Consumers.
        builder.Services
            .AddMessageBrokerWithMassTransitForApi(
                builder.Configuration,
                Assembly.GetExecutingAssembly());

        // Configures and registers Serilog as logger.
        builder.UseSerilogCustom();

        // HealthChecks.
        builder.Services.AddHealthChecksCustom();

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerCustom("v1", "EmailService", "EmailService microservice.");

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
}
