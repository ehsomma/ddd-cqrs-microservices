using System.Reflection;

#pragma warning disable IDE0130 // Namespace does not match folder structure
//// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.Configuration;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
/// Extension method for IConfiguration.
/// </summary>
public static class ConfigurationExtensions
{
    /// <summary>
    /// Gets a configuration sub-section with the specified <paramref name="key"/>.
    /// </summary>
    /// <param name="configuration">The IConfiguration to extend.</param>
    /// <param name="key">The key of the configuration section.</param>
    /// <typeparam name="TSettings">The type of the new instance to bind.</typeparam>
    /// <returns>The <see cref="IConfigurationSection"/>.</returns>
    /// <remarks>
    ///     This method will never return <c>null</c>. If no matching sub-section is found with the specified key,
    ///     an empty <see cref="IConfigurationSection"/> will be returned.
    /// </remarks>
    public static TSettings GetSectionOrThrow<TSettings>(this IConfiguration configuration, string key)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        TSettings? settings = configuration.GetSection(key).Get<TSettings>();

        if (settings == null)
        {
            string assembly = Assembly.GetEntryAssembly()?.FullName ?? "(unresolved)";
            throw new Exception($"Configuration section '{key}' not found in assemby {assembly}.");
        }

        return settings;
    }
}
