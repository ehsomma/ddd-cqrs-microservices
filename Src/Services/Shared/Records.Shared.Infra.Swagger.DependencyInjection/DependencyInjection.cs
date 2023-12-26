#region Usings

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Records.Shared.Infra.Swagger.Schemas;

#endregion

namespace Records.Shared.Infra.Swagger.DependencyInjection;

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
    /// <param name="version">The version of the OpenAPI document.</param>
    /// <param name="title">The title of the application.</param>
    /// <param name="description">A short description of the application.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddSwaggerCustom(
        this IServiceCollection services,
        string version,
        string title,
        string description)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddSwaggerGen(options =>
        {
            // FIX: To prevent swagger error* when there are different classes with same
            // names (even though they are in different namespaces).
            //
            // *Error: "Can't use schemaId "$Person" for type *. The same schemaId is
            // already used for type *.".
            // Source: https://stackoverflow.com/a/63331752/15711876.
            options.CustomSchemaIds(type => type.ToString());

            options.SwaggerDoc(version, new OpenApiInfo
            {
                Version = version,
                Title = title,
                Description = description,
                /*
                TermsOfService = new Uri("https://example.com/terms"),
                Contact = new OpenApiContact
                {
                    Name = "Example Contact",
                    Url = new Uri("https://example.com/contact"),
                },
                License = new OpenApiLicense
                {
                    Name = "Example License",
                    Url = new Uri("https://example.com/license"),
                },
                */
            });

            // Configure Swagger to use the xml documentation files (for Linux OS).
            // Adding the xml documentation of the Main project.
            ////string xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            ////options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

            // Adding the xml documentation of the Sub project.
            ////string xmlFilenameSubProject = Path.Combine(AppContext.BaseDirectory, "Records.Countries.Contracts.xml");
            ////options.IncludeXmlComments(xmlFilenameSubProject);

            // Adding the xml documentation of all projects (main and sub projects).
            // Source: https://www.gopiportal.in/2018/05/swagger-integration-including-xml.html.
            DirectoryInfo directoryInfo = new (AppContext.BaseDirectory);
            foreach (FileInfo fileInfo in directoryInfo.EnumerateFiles("*.xml"))
            {
                options.IncludeXmlComments(fileInfo.FullName);
            }

            // Swagger example attribute.
            options.SchemaFilter<SwaggerSchemaExampleFilter>();
        });

        return services;
    }

    /// <summary>
    /// Register the SwaggerUI middleware with provided options.
    /// </summary>
    /// <param name="app">The <see cref="IApplicationBuilder"/> to add the middleware to.</param>
    /// <returns>The same application builder.</returns>
    public static IApplicationBuilder UseSwaggerUICustom(this WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);

        app.UseSwagger();

        app.UseSwaggerUI(options =>
        {
            // Shows swagger in "https://localhost:<port>/" instead "https://localhost:<port>/swagger".
            ////options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            ////options.RoutePrefix = string.Empty;
        });

        return app;
    }

    #endregion
}
