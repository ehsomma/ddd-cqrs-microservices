#region Usings

using MediatR;
using Microsoft.Extensions.Options;
using Records.Persons.Domain.Countries.Services;
using Records.Persons.Domain.Persons.Enumerators;
using Records.Persons.Domain.Persons.Repositories;
using Records.Persons.Domain.Persons.ValueObjects;
using Records.Persons.Domain.Shared.ValueObjects;
using Records.Shared.Application.Handlers;
using Records.Shared.Domain.ValueObjects;
using Records.Shared.Infra.Persistence.Abstractions;
using DomainModel = Records.Persons.Domain.Persons.Models; // Using aliases.
using DomainModelShared = Records.Persons.Domain.Shared.Models; // Using aliases.
using Dto = Records.Persons.Dtos.Persons; // Using aliases.
using DtoShared = Records.Persons.Dtos.Shared; // Using aliases.

#endregion

namespace Records.Persons.Application.Persons.Commands.CreatePerson;

/// <summary>
/// Represents the <see cref="CreatePersonCommand"/> handler.
/// </summary>
internal sealed class CreatePersonCommandHandler : CommandHandler<CreatePersonCommand>
{
    #region Declarations

    private readonly DtoShared.PersonsSettings _settings;
    private readonly IPersonRepository _personRepository;
    private readonly ICountryService _countryService;

    #endregion

    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CreatePersonCommandHandler"/> class.
    /// </summary>
    /// <param name="mediator">MediatR library to send and handle commands and queries implementing CQRS.</param>
    /// <param name="unitOfWork">Manage a <see cref="IDbSession"/> to encapsulate a business transaction which can affect the database.</param>
    /// <param name="settings">Represents the settings that will be mapped from the Persons key in the appsettings.json file.</param>
    /// <param name="personRepository">Represents the repository for <see cref="DomainModel.Person"/>.</param>
    /// <param name="countryService">Represents the <see cref="Records.Persons.Domain.Countries.Models.Country"/> service.</param>
    /// <exception cref="ArgumentNullException">When some argument for the constructor parameters is null.</exception>
    public CreatePersonCommandHandler(
        IMediator mediator,
        IUnitOfWork unitOfWork,
        IOptionsSnapshot<DtoShared.PersonsSettings> settings,
        IPersonRepository personRepository,
        ICountryService countryService)
        : base(mediator, unitOfWork)
    {
        ArgumentNullException.ThrowIfNull(settings);

        _settings = settings.Value;
        _personRepository = personRepository ?? throw new ArgumentNullException(nameof(personRepository));
        _countryService = countryService ?? throw new ArgumentNullException(nameof(countryService));
    }

    #endregion

    #region Public methods

    /// <inheritdoc />
    public override async Task Handle(CreatePersonCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        // NOTE: Just if we need some Persons settings.
        ////DomainModelShared.PersonsSettings domainSettings = LoadSettings(_settings);

        DomainModel.Person person = CreatePerson(command);

        await _countryService.EnsureExists(person.Address);

        _unitOfWork.BeginTransaction();
        await _personRepository.InsertAsync(person);
        _unitOfWork.Commit();

        await PublishDomainEvents(person.PullDomainEvents());
    }

    #endregion

    #region Private methods

    private DomainModelShared.PersonsSettings LoadSettings(DtoShared.PersonsSettings dtoSetting)
    {
        ArgumentNullException.ThrowIfNull(nameof(dtoSetting));

        // Example to create "domain" settings from normal settings. The domain settings will be created
        // and will ensure that all settings that require a value have it or, to not required settings,
        // it will set a default value if they do not have one.
        DomainModelShared.PersonsSettings personsSettings = DomainModelShared.PersonsSettings.Create(
            dtoSetting.Setting1,
            dtoSetting.Setting2);

        return personsSettings;
    }

    private DomainModel.Person CreatePerson(CreatePersonCommand command)
    {
        ArgumentNullException.ThrowIfNull(nameof(command));

        Dto.Person personDto = command.Person;
        Dto.Address? personAddressDto = command.Person.Address;

        DomainModel.Address? domainAddress = null;
        if (personAddressDto != null)
        {
            LatLng? domainLatLng = null;
            if (personAddressDto.LatLng != null)
            {
                domainLatLng = LatLng.Build(personAddressDto.LatLng.Lat, personAddressDto.LatLng.Lng);
            }

            domainAddress = DomainModel.Address.Create(
                StreetLine.Build(personAddressDto.StreetLine1),
                StreetLine2.Build(personAddressDto.StreetLine2),
                City.Build(personAddressDto.City),
                State.Build(personAddressDto.State),
                CountryName.Build(personAddressDto.Country),
                domainLatLng);
        }

        FullName fullName = FullName.Build(personDto.FullName);
        Email email = Email.Build(personDto.Email);
        PhoneNumber phoneNumber = PhoneNumber.Build(personDto.Phone);

        DomainModel.Person person = DomainModel.Person.Create(
            personDto.Id,
            domainAddress,
            fullName,
            email,
            phoneNumber,
            Gender.FromName(personDto.Gender),
            null,
            personDto.Birthdate);

        if (personDto.PersonalAssets != null)
        {
            foreach (Dto.PersonalAsset personalAssetDto in personDto.PersonalAssets)
            {
                DomainModel.PersonalAsset domainPersonalAsset = DomainModel.PersonalAsset.Create(
                    PersonalAssetDescription.Build(personalAssetDto.Description),
                    Money.Build(personalAssetDto.Value));
                person.AddPersonalAsset(domainPersonalAsset);
            }
        }

        return person;
    }

    #endregion
}
