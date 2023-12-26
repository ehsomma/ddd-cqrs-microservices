#region Usings

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Records.Shared.Infra.Http.Middlewares;

#endregion

namespace Records.Shared.Infra.Http.DependencyInjection;

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
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddGlobalExceptionHandler(this IServiceCollection services)
    {
        services.AddTransient<GlobalExceptionHandlerMiddleware>();

        return services;
    }

    /// <summary>
    /// Configure the custom exception handler middleware.
    /// </summary>
    /// <param name="app">The <see cref="IApplicationBuilder"/> to add the middleware to.</param>
    /// <returns>The configured application builder.</returns>
    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(app);

        return app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
    }

    #endregion
}
