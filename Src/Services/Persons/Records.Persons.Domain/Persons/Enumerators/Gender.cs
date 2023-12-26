using Records.Shared.Domain.Models;

namespace Records.Persons.Domain.Persons.Enumerators;

/// <summary>
/// Represents the Gender enumeration.
/// </summary>
public sealed class Gender : Enumeration<Gender>
{
    #region Declarations

    /// <summary>Male gender.</summary>
    public static readonly Gender Male = new (1, nameof(Male)); // nameof(Male).ToLowerInvariant()

    /// <summary>Female gender.</summary>
    public static readonly Gender Female = new (2, nameof(Female).ToLowerInvariant()); // nameof(Male).ToLowerInvariant()

    /// <summary>Other gender.</summary>
    public static readonly Gender Other = new (3, nameof(Other).ToLowerInvariant()); // nameof(Male).ToLowerInvariant()
    ////public static readonly Gender DescripWithSpaces = new Gender(4, "Descrip with spaces"); // Example to use descript with spaces!

    #endregion

    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="Gender"/> class.
    /// </summary>
    /// <param name="value">The enumeration value.</param>
    /// <param name="name">The enumeration name.</param>
    private Gender(int value, string name)
        : base(value, name)
    {
    }

    #endregion
}
