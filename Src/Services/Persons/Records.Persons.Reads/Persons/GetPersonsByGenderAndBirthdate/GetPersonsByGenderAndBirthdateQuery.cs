namespace Records.Persons.Reads.Persons.GetPersonsByGenderAndBirthdate;

/// <summary>
/// Represents a query to get Persons corresponding to Gender and birthdate period filters.
/// </summary>
public sealed class GetPersonsByGenderAndBirthdateQuery
{
    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="GetPersonsByGenderAndBirthdateQuery"/> class.
    /// </summary>
    /// <param name="gender">The gender of the Person.</param>
    /// <param name="dateFrom">Birthdate from.</param>
    /// <param name="dateTo">Birthdate to.</param>
    public GetPersonsByGenderAndBirthdateQuery(string gender, DateTime dateFrom, DateTime dateTo)
    {
        Gender = gender;
        DateFrom = dateFrom;
        DateTo = dateTo;
    }

    #endregion

    #region Properties

    /// <summary>The gender of the Person.</summary>
    public string Gender { get; init; }

    /// <summary>Birthdate from.</summary>
    public DateTime DateFrom { get; init; }

    /// <summary>Birthdate to.</summary>
    public DateTime DateTo { get; init; }

    #endregion
}
