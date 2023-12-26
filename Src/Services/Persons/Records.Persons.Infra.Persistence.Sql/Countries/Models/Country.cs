using Dapper.Contrib.Extensions;

namespace Records.Persons.Infra.Persistence.Sql.Countries.Models;

/// <summary>
/// Represents the Country data model for table Countries.
/// </summary>
[Table("Countries")]
public class Country
{
    #region Properties

    /// <summary>Country IATA code.</summary>
    [ExplicitKey]
    public string? IataCode { get; init; }

    /// <summary>Country name.</summary>
    public string? Name { get; init; }

    #endregion
}
