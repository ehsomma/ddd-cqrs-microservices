namespace Records.Persons.Reads.Persons.GetPersonsWithSpecificProfile;

/// <summary>
/// Represents a query to get Persons corresponding to the Gender, AgeOlderThan and LiveInCity filters.
/// </summary>
public sealed class GetPersonsWithSpecificProfileQuery
{
    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="GetPersonsWithSpecificProfileQuery"/> class.
    /// </summary>
    /// <param name="gender">The gender of the Person.</param>
    /// <param name="ageOlderThan">Minimum age.</param>
    /// <param name="liveInCity">Name of the city.</param>
    public GetPersonsWithSpecificProfileQuery(
        string gender,
        int ageOlderThan,
        string liveInCity)
    {
        Gender = gender;
        AgeOlderThan = ageOlderThan;
        LiveInCity = liveInCity;
    }

    #endregion

    #region Properties

    /// <summary>The gender of the Person.</summary>
    public string Gender { get; init; }

    /// <summary>Minimum age.</summary>
    public int AgeOlderThan { get; init; }

    /// <summary>Name of the city.</summary>
    public string LiveInCity { get; init; }

    #endregion
}
