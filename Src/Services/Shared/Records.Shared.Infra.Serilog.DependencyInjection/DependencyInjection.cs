#region Usings

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Core;
using Serilog.Enrichers;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Exceptions.Core;

#endregion

namespace Records.Shared.Infra.Serilog.DependencyInjection;

/// <summary>
/// Extensions methods for dependency injection.
/// </summary>
public static class DependencyInjection
{
    #region Public methods

    /// <summary>
    /// Configures and registers Serilog as logger.
    /// </summary>
    /// <param name="builder">The web application and servces builder.</param>
    /// <returns>The web application and services builder.</returns>
    public static WebApplicationBuilder UseSerilogCustom(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder, nameof(builder));

        // LoggingLevelSwitch allow to change the level in runtime.
        LoggingLevelSwitch loggingLevelSwitch = new ()
        {
            MinimumLevel = LogEventLevel.Information,
        };

        // Adds the loggingLevelSwitch to the service collection so we can inject it in a api
        // controller to change the level from and api endpoint.
        builder.Services.AddSingleton<LoggingLevelSwitch>(loggingLevelSwitch);

        const string logTemplate = @"{Timestamp:yyyy-MM-dd HH:mm:ss zzz} Level:{Level:u4} ThreadId:{ThreadId} [{SourceContext:l}] '{Message:lj}' {Properties:j}{NewLine}";

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.ControlledBy(loggingLevelSwitch) ////.MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("System", LogEventLevel.Warning)
            .Enrich.With(new ThreadIdEnricher())
            .Enrich.FromLogContext()
            .Enrich.WithExceptionDetails(new DestructuringOptionsBuilder().WithDefaultDestructurers())
            ////.WriteTo.File("log.txt", LogEventLevel.Warning, logTemplate)
            .WriteTo.Console(LogEventLevel.Information, logTemplate)
            .CreateLogger();

        builder.Host.UseSerilog();

        // Reads the configuration from appsettings.json.
        // NOTE: Serilog appsettings.json configuration doesn't work with Serilog.Exceptions
        // and I whant to use it. So, I'm using configuration by code.
        /*
        hostBuilder.UseSerilog((context, configuration) =>
        {
            configuration.ReadFrom.Configuration(context.Configuration);
        });
        */

        return builder;
    }

    #endregion
}
