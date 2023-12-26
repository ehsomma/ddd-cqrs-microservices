#region Usings

using Mapster;
using Records.Persons.Domain.Persons.Enumerators;
using Records.Persons.Domain.Persons.ValueObjects;
using Records.Persons.Domain.Shared.ValueObjects;
using Records.Shared.Domain.ValueObjects;
using Records.Shared.Infra.Persistence.Mappings.Abstractions.Mappers;
using DataModel = Records.Persons.Infra.Persistence.Sql.Persons.Models; // Using aliases.
using DomainModel = Records.Persons.Domain.Persons.Models; // Using aliases.

#endregion

namespace Records.Persons.Infra.Persistence.Sql.Persons.Mappers;

/// <summary>
/// Defines a mapper to map a <see cref="DomainModel.Person"/> to a <see cref="DataModel.Person"/>
/// and vice versa.
/// </summary>
public class PersonMapper : IPersistanceMapper<DomainModel.Person, DataModel.Person>
{
    #region Public methods

    /// <inheritdoc />
    public DataModel.Person FromDomainToDataModel(DomainModel.Person domainCountry)
    {
        DataModel.Person dataPerson = domainCountry.Adapt<DataModel.Person>();

        // FIX: I can't resolve it with Mapster.
        // Set the PersonId to each personal asset.
        foreach (DataModel.PersonalAsset personalAsset in dataPerson.PersonalAssets!.NotNull())
        {
            personalAsset.PersonId = domainCountry.Id;
        }

        // FIX2: Now I can! See PersonMapperConfig class.
        // NOTE: FIX2 not working.
        ////.Map(dest => dest.PersonalAssets, src => new List<DataModel.PersonalAsset>() { new DataModel.PersonalAsset() { PersonId = src.Id } });

        return dataPerson;
    }

    /// <inheritdoc />
    public DomainModel.Person? FromDataModelToDomain(DataModel.Person? dataCountry)
    {
        DomainModel.Person? domainPerson = null;

        if (dataCountry != null)
        {
            // DomainModel.Value.
            DomainModel.Address? domainAddress = null;
            if (dataCountry.Address != null)
            {
                DataModel.Address dataAddress = dataCountry.Address;
                domainAddress = DomainModel.Address.Load(
                    dataAddress.Id,
                    StreetLine.Build(dataAddress.StreetLine1),
                    StreetLine2.Build(dataAddress.StreetLine2),
                    City.Build(dataAddress.City),
                    State.Build(dataAddress.State),
                    CountryName.Build(dataAddress.Country),
                    LatLng.Build(dataAddress.Lat, dataAddress.Lng));
            }

            FullName fullName = FullName.Build(dataCountry.FullName);
            Email email = Email.Build(dataCountry.Email);
            PhoneNumber phoneNumber = PhoneNumber.Build(dataCountry.Phone);

            // DomainModel.Person.
            domainPerson = DomainModel.Person.Load(
                dataCountry.Id,
                domainAddress,
                fullName,
                email,
                phoneNumber,
                Gender.FromName(dataCountry.Gender),
                null,
                dataCountry.Birthdate,
                dataCountry.CreatedOnUtc,
                dataCountry.UpdatedOnUtc);

            // DomainModel.PersonalAsset.
            if (dataCountry.PersonalAssets != null)
            {
                foreach (DataModel.PersonalAsset dataPersonalAsset in dataCountry.PersonalAssets)
                {
                    DomainModel.PersonalAsset domainPersonalAsset = DomainModel.PersonalAsset.Load(
                        dataPersonalAsset.Id,
                        PersonalAssetDescription.Build(dataPersonalAsset.Description),
                        Money.Build(dataPersonalAsset.Value));

                    domainPerson.LoadPersonalAsset(domainPersonalAsset);
                }
            }
        }

        return domainPerson;
    }

    #endregion
}
