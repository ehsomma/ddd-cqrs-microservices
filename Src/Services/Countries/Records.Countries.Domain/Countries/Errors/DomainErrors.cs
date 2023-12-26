using Records.Shared.Domain.Models;

namespace Records.Countries.Domain.Countries.Errors;

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
            "Country no found.",
            "Domain");

        /// <summary>When a Country already exists.</summary>
        public static Error AlreadyExist => new (
            "ERR.COUNTRY.ALREADYEXISTS",
            "Country already exists.",
            "Domain");

        #endregion
    }
}
