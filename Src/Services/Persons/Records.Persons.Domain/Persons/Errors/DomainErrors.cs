using Records.Shared.Domain.Models;

namespace Records.Persons.Domain.Persons.Errors;

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
    /// Contains the domain errors for <see cref="Person"/>.
    /// </summary>
    public static class Person
    {
        #region Errors

        /// <summary>When a Person not found.</summary>
        public static Error NotFound => new (
            "ERR.PERSON.NOTFOUND",
            "Person not found.",
            "Domain");

        /// <summary>When a person address have an invalid country.</summary>
        public static Error InvalidCountry => new (
            "ERR.PERSON.INVALIDCOUNTRY",
            "Country not found, invalid.",
            "Domain");

        #endregion
    }
}
