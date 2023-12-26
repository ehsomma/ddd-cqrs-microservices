#region Usings

using Mapster;
using DataModel = Records.Persons.Infra.Persistence.Sql.Persons.Models; // Using aliases.
using DomainModel = Records.Persons.Domain.Persons.Models; // Using aliases.

#endregion

namespace Records.Persons.Infra.Persistence.Sql.Persons.Mappers;

/// <summary>
/// Represents the Mapster configuration for <see cref="PersonMapper"/>.
/// </summary>
internal class PersonMapperConfig : IRegister
{
    #region Public methods

    /// <inheritdoc />
    public void Register(TypeAdapterConfig config)
    {
        // DomainModel.Person => DataModel.Person.
        config.NewConfig<DomainModel.Person, DataModel.Person>()
            .Map(dest => dest.Id, src => src.Id.ToString())
            .Map(dest => dest.Address!.PersonId, src => src.Id.ToString());

        config.NewConfig<DomainModel.Address, DataModel.Address>()
            .Map(dest => dest.Lat, src => src.LatLng!.Lat)
            .Map(dest => dest.Lng, src => src.LatLng!.Lng);

        //// https://stackoverflow.com/questions/75872400/mapster-how-to-map-parent-property-to-a-list.
        ////config.NewConfig<DomainModel.PersonalAsset, DataModel.PersonalAsset>()
        ////    .Map(dest => dest.PersonId, src => "asdasdsad"); // !!!???.
        ////config.NewConfig<DomainModel.Person, DataModel.Person>()
        ////    .Map(dest => dest.PersonalAssets,
        ////        src => new List<DataModel.PersonalAsset>() { new DataModel.PersonalAsset() { PersonId = src.Id } });

        // FIX: Not working.
        // DomainModel.Person => DataModel.Person.
        ////config.NewConfig<DomainModel.Person, DataModel.Person>()
        ////    .Map(dest => dest.Id, src => src.Id)
        ////    .Map(dest => dest.Value!.PersonId, src => src.Id)
        ////    .Map(dest => dest.PersonalAssets, src => new List<DataModel.PersonalAsset>() { new DataModel.PersonalAsset() { PersonId = src.Id } });
    }

    #endregion
}
