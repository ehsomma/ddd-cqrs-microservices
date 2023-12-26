using Records.HealthChecker.HealthCheck;

namespace Records.HealthChecker;

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

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        ////builder.Services.AddSwaggerGen();

        // HealthChecks.
        builder.Services.AddHealthChecksCustom();

        WebApplication app = builder.Build();

        ////app.UseSwagger();
        ////app.UseSwaggerUI();
        app.UseHttpsRedirection();
        app.UseAuthorization();

        // HealthChecks.
        app.UseHealthChecksCustom();

        app.MapControllers();

        app.Run();
    }

    #endregion
}
