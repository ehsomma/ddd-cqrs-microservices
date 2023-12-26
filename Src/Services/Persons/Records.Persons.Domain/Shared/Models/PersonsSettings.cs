#region Usings

using Records.Shared.Domain.Exceptions;
using Throw;

#endregion

namespace Records.Persons.Domain.Shared.Models;

/// <summary>
/// Represents the settings (with domain rures) that will be mapped from the Persons key in
/// the appsettings.json file.
/// </summary>
/// <remarks>
/// The domain settings will be created and will ensure that all settings that require a value have
/// it or, to not required settings, it will set a default value if they do not have one.
/// </remarks>
[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "StyleCop.CSharp.OrderingRules",
    "SA1201:Elements should appear in the correct order",
    Justification = "Prefere to have the Create() inside the constructor region.")]
public sealed class PersonsSettings
{
    #region Definitions

    private const string KeyNotFound = "The '{0}' configuration key was not found.";

    #endregion

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="PersonsSettings"/> class.
    /// </summary>
    /// <param name="setting1">The setting1 (example).</param>
    /// <param name="setting2">The setting2 (example).</param>
    private PersonsSettings(string setting1, string setting2)
    {
        EnsureSetting1IsValid(setting1);
        EnsureSetting2IsValidOrDefault(ref setting2);

        Setting1 = setting1;
        Setting2 = setting2;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="PersonsSettings"/> class and ensure all its required
    /// properties are valid.
    /// </summary>
    /// <param name="setting1">The setting1 (example).</param>
    /// <param name="setting2">The setting2 (example).</param>
    /// <returns>A <see cref="PersonsSettings"/> with all its required properties validated.</returns>
    public static PersonsSettings Create(string setting1, string setting2)
    {
        PersonsSettings settings = new (setting1, setting2);
        return settings;
    }

    #endregion

    #region Properties

    /// <summary>The setting1 (example).</summary>
    public string Setting1 { get; private set; }

    /// <summary>The setting2 (example).</summary>
    public string Setting2 { get; private set; }

    #endregion

    #region Privated methods

    private void EnsureSetting1IsValid(string setting1)
    {
        try
        {
            setting1.ThrowIfNull().IfEmpty().IfWhiteSpace();
        }
        catch (Exception ex)
        {
            // If it doesn't exist, throws an exception with a detailed message (required setting).
            throw new DomainValidationException(string.Format(KeyNotFound, "Setting1"), ex);
        }
    }

    private void EnsureSetting2IsValidOrDefault(ref string setting2)
    {
        if (string.IsNullOrEmpty(setting2))
        {
            setting2 = "Setting value 2 (default)";
        }
    }

    #endregion
}
