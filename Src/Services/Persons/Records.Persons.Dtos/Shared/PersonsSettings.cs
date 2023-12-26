namespace Records.Persons.Dtos.Shared;

/// <summary>
/// Represents the settings that will be mapped from the Persons key in the appsettings.json file.
/// </summary>
public class PersonsSettings
{
    #region Declarations

    /// <summary>The key to map from the appsettings.json file.</summary>
    public const string SettingsKey = "Persons"; // Without "...Settings" suffix.

    #endregion

    #region Properties

    /// <summary>Setting 1 (example).</summary>
    public string Setting1 { get; set; } = string.Empty;

    /// <summary>Setting 2 (example).</summary>
    public string Setting2 { get; set; } = string.Empty;

    #endregion
}
