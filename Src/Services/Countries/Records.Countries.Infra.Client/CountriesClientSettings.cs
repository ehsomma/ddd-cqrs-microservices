namespace Records.Countries.Infra.Client;

/// <summary>
/// Represents the settings that will be mapped from the CountriesClient key in the appsettings.json file.
/// </summary>
public class CountriesClientSettings
{
    #region Declarations

    /// <summary>The key to map from the appsettings.json file.</summary>
    public const string SettingsKey = "CountriesClient"; // Without "...Settings" suffix.

    #endregion

    #region Properties

    /// <summary>The API base uri.</summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Design",
        "CA1056:URI-like properties should not be strings",
        Justification = "TODO: Use System.Uri not string.")]
    public string BaseUri { get; init; } = string.Empty;

    /// <summary>An example property.</summary>
    public string HeaderExample1 { get; init; } = string.Empty;

    #endregion
}
