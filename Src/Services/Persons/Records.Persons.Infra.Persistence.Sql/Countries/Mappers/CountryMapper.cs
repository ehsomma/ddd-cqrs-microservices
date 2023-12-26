#region Usings

using Mapster;
using Records.Persons.Domain.Countries.ValueObjects;
using Records.Persons.Domain.Shared.ValueObjects;
using Records.Shared.Infra.Persistence.Mappings.Abstractions.Mappers;
using DataModel = Records.Persons.Infra.Persistence.Sql.Countries.Models; // Using aliases.
using DomainModel = Records.Persons.Domain.Countries.Models; // Using aliases.

#endregion

namespace Records.Persons.Infra.Persistence.Sql.Countries.Mappers;

/// <summary>
/// Defines a mapper to map a <see cref="DomainModel.Country"/> to a <see cref="DataModel.Country"/>
/// and vice versa.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Naming",
    "CA1725:Parameter names should match base declaration",
    Justification = "Base declaration is damainModel we prefere to use damainXxxx here.")]
public class CountryMapper : IPersistanceMapper<DomainModel.Country, DataModel.Country>
{
    #region Public methods

    /// <inheritdoc />
    public DataModel.Country FromDomainToDataModel(DomainModel.Country domainCountry)
    {
        ArgumentNullException.ThrowIfNull(nameof(domainCountry));

        DataModel.Country dataCountry = domainCountry.Adapt<DataModel.Country>();

        return dataCountry;
    }

    /// <inheritdoc />
    public DomainModel.Country? FromDataModelToDomain(DataModel.Country? dataCountry)
    {
        // NOTE: Accepts null.
        ////ArgumentNullException.ThrowIfNull(nameof(dataCountry));

        DomainModel.Country? domainCountry = null;

        if (dataCountry != null)
        {
            CountryIataCode iataCode = CountryIataCode.Build(dataCountry.IataCode);
            CountryName name = CountryName.Build(dataCountry.Name);

            domainCountry = DomainModel.Country.Load(iataCode, name);
        }

        return domainCountry;
    }

    #endregion
}
