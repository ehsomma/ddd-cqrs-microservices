using Records.Shared.Projection.Abstractions;

namespace Records.Persons.Projection.Countries.Projectors;

/// <summary>
/// Defines the CountryDeletedProjector. Projectors project (copy) data from "source" database to
/// "projection" (read) database.
/// </summary>
public interface ICountryDeletedProjector : IProjector
{
    /// <summary>
    /// Projects the delete of the Country corresponding to the specifies IATA code.
    /// </summary>
    /// <param name="iataCode">The IATA code to search for.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task ProjectAsync(string iataCode);
}
