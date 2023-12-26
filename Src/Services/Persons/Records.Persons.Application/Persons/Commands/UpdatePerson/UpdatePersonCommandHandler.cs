#region Usings

using MediatR;
using Microsoft.Extensions.Options;
using Records.Persons.Domain.Countries.Repositories;
using Records.Persons.Domain.Countries.Services;
using Records.Persons.Domain.Persons.Enumerators;
using Records.Persons.Domain.Persons.Models;
using Records.Persons.Domain.Persons.Repositories;
using Records.Persons.Domain.Persons.Services;
using Records.Persons.Domain.Persons.ValueObjects;
using Records.Persons.Domain.Shared.ValueObjects;
using Records.Shared.Application.Handlers;
using Records.Shared.Domain.ValueObjects;
using Records.Shared.Infra.Persistence.Abstractions;
using DomainModel = Records.Persons.Domain.Persons.Models; // Using aliases.
////using DomainModelShared = Records.Persons.Domain.Shared.Models; // Using aliases.
using Dto = Records.Persons.Dtos.Persons; // Using aliases.
using DtoShared = Records.Persons.Dtos.Shared; // Using aliases.

#endregion

namespace Records.Persons.Application.Persons.Commands.UpdatePerson;

/// <summary>
/// Represents the <see cref="UpdatePersonCommand"/> handler.
/// </summary>
internal sealed class UpdatePersonCommandHandler : CommandHandler<UpdatePersonCommand>
{
    #region Declarations

    private readonly DtoShared.PersonsSettings _settings;
    private readonly IPersonRepository _personRepository;
    private readonly IPersonService _personService;
    private readonly ICountryService _countryService;

    #endregion

    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdatePersonCommandHandler"/> class.
    /// </summary>
    /// <param name="mediator">MediatR library to send and handle commands and queries implementing CQRS.</param>
    /// <param name="unitOfWork">Manage a <see cref="IDbSession"/> to encapsulate a business transaction which can affect the database.</param>
    /// <param name="personRepository">Represents the repository for <see cref="DomainModel.Person"/>.</param>
    /// <param name="countryRepository">Represents the repository for <see cref="Records.Persons.Domain.Countries.Models.Country"/>.</param>
    /// <param name="personService">Represents the <see cref="DomainModel.Person"/> service.</param>
    /// <param name="countryService">Represents the <see cref="Records.Persons.Domain.Countries.Models.Country"/> service.</param>
    /// <param name="settings">Represents the settings that will be mapped from the Persons key in the appsettings.json file.</param>
    /// <exception cref="ArgumentNullException">When some argument for the constructor parameters is null.</exception>
    public UpdatePersonCommandHandler(
        IMediator mediator,
        IUnitOfWork unitOfWork,
        IPersonRepository personRepository,
        ICountryRepository countryRepository,
        IPersonService personService,
        ICountryService countryService,
        IOptionsSnapshot<DtoShared.PersonsSettings> settings)
        : base(mediator, unitOfWork)
    {
        ArgumentNullException.ThrowIfNull(settings);

        _settings = settings.Value;
        _personRepository = personRepository ?? throw new ArgumentNullException(nameof(personRepository));
        _personService = personService ?? throw new ArgumentNullException(nameof(personService));
        _countryService = countryService ?? throw new ArgumentNullException(nameof(countryService));
    }

    #endregion

    #region Public methods

    /// <inheritdoc />
    public override async Task Handle(UpdatePersonCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        // Loads a DomainModel.Person with the edited data coming from the command (FromCommandToDomain).
        Person editedPerson = LoadPerson(command);

        await _countryService.EnsureExists(editedPerson.Address);

        // Gets the current DomainModel.Person.
        Person? currentPerson = await _personService.GetByIdAsync(editedPerson.Id);

        // Updates the currentPerson with editedPerson data.
        currentPerson!.Update(editedPerson);

        _unitOfWork.BeginTransaction();
        await _personRepository.UpdateAsync(currentPerson);
        _unitOfWork.Commit();

        await PublishDomainEvents(currentPerson.PullDomainEvents());
    }

    #endregion

    #region Private methods

    private Person LoadPerson(UpdatePersonCommand command)
    {
        Dto.Person personDto = command.Person;
        Dto.Address? personAddressDto = command.Person.Address;

        Address? domainAddress = null;
        if (personAddressDto != null)
        {
            LatLng? domainLatLng = null;
            if (personAddressDto.LatLng != null)
            {
                domainLatLng = LatLng.Build(personAddressDto.LatLng.Lat, personAddressDto.LatLng.Lng);
            }

            domainAddress = Address.Load(
                personAddressDto.Id,
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

        Person person = Person.Load(
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
                PersonalAsset domainPersonalAsset = PersonalAsset.Load(
                    personalAssetDto.Id,
                    PersonalAssetDescription.Build(personalAssetDto.Description),
                    Money.Build(personalAssetDto.Value));
                person.LoadPersonalAsset(domainPersonalAsset);
            }
        }

        return person;
    }

    #endregion
}
