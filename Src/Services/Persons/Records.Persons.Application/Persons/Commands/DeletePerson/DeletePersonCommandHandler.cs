#region Usings

using MediatR;
using Microsoft.Extensions.Options;
using Records.Persons.Domain.Persons.Repositories;
using Records.Persons.Domain.Persons.Services;
using Records.Shared.Application.Handlers;
using Records.Shared.Infra.Persistence.Abstractions;
using DomainModel = Records.Persons.Domain.Persons.Models; // Using aliases.
////using DomainModelShared = Records.Persons.Domain.Shared.Models; // Using aliases.
////using Dto = Records.Persons.Dtos.Persons; // Using aliases.
using DtoShared = Records.Persons.Dtos.Shared; // Using aliases.

#endregion

namespace Records.Persons.Application.Persons.Commands.DeletePerson;

/// <summary>
/// Represents the <see cref="DeletePersonCommand"/> handler.
/// </summary>
internal sealed class DeletePersonCommandHandler : CommandHandler<DeletePersonCommand>
{
    #region Declarations

    private readonly DtoShared.PersonsSettings _settings;
    private readonly IPersonRepository _personRepository;
    private readonly IPersonService _personService;

    #endregion

    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="DeletePersonCommandHandler"/> class.
    /// </summary>
    /// <param name="mediator">MediatR library to send and handle commands and queries implementing CQRS.</param>
    /// <param name="unitOfWork">Manage a <see cref="IDbSession"/> to encapsulate a business transaction which can affect the database.</param>
    /// <param name="settings">Represents the settings that will be mapped from the Persons key in the appsettings.json file.</param>
    /// <param name="personRepository">Represents the repository for <see cref="DomainModel.Person"/>.</param>
    /// <param name="personService">Represents the <see cref="DomainModel.Person"/> service.</param>
    /// <exception cref="ArgumentNullException">When some argument for the constructor parameters is null.</exception>
    public DeletePersonCommandHandler(
        IMediator mediator,
        IUnitOfWork unitOfWork,
        IOptionsSnapshot<DtoShared.PersonsSettings> settings,
        IPersonRepository personRepository,
        IPersonService personService)
        : base(mediator, unitOfWork)
    {
        ArgumentNullException.ThrowIfNull(settings);

        _settings = settings.Value;
        _personRepository = personRepository ?? throw new ArgumentNullException(nameof(personRepository));
        _personService = personService ?? throw new ArgumentNullException(nameof(personService));
    }

    #endregion

    #region Public methods

    /// <inheritdoc />
    public override async Task Handle(DeletePersonCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        // NOTE: Just if we need some Persons settings (see CreatePersonCommandHandler).
        ////DomainModelShared.PersonsSettings domainSettings = this.LoadSettings(_settings);

        // Gets the current DomainModel.Person.
        DomainModel.Person? person = await _personService.GetByIdAsync(command.PersonId);

        // Deletes the Person from domain.
        person!.Delete();

        _unitOfWork.BeginTransaction();
        await _personRepository.DeleteAsync(person);
        _unitOfWork.Commit();

        await PublishDomainEvents(person.PullDomainEvents());
    }

    #endregion
}
