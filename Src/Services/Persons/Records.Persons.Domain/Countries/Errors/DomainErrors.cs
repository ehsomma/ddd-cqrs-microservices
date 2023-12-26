using Records.Shared.Domain.Models;

namespace Records.Persons.Domain.Countries.Errors;

/// <summary>
/// Contains the domain errors.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Design",
    "CA1034:Nested types should not be visible",
    Justification = "It is just for intellisense.")]
public static class DomainErrors
{
    /// <summary>
    /// Contains the domain errors for <see cref="Country"/>.
    /// </summary>
    public static class Country
    {
        #region Errors

        /// <summary>When a Country not found.</summary>
        public static Error NotFound => new (
            "ERR.COUNTRY.NOTFOUND",
            "Country not found.",
            "Domain");

        #endregion
    }
}
