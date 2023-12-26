#region Usings

using MediatR;
using Records.Persons.Domain.Countries.Repositories;
using Records.Persons.Domain.Countries.Services;
using Records.Persons.Domain.Countries.ValueObjects;
using Records.Shared.Application.Handlers;
using Records.Shared.Infra.Persistence.Abstractions;
using DomainModel = Records.Persons.Domain.Countries.Models; // Using aliases.

#endregion

namespace Records.Persons.Application.Countries.Commands.DeleteCountry;

/// <summary>
/// Represents the <see cref="DeleteCountryCommand"/> handler.
/// </summary>
internal sealed class DeleteCountryCommandHandler : CommandHandler<DeleteCountryCommand>
{
    #region Declarations

    /// <summary>Represents the repository for <see cref="DomainModel.Country"/>.</summary>
    private readonly ICountryRepository _countryRepository;

    #endregion

    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteCountryCommandHandler"/> class.
    /// </summary>
    /// <param name="mediator">MediatR library to send and handle commands and queries implementing CQRS.</param>
    /// <param name="unitOfWork">Manage a <see cref="IDbSession"/> to encapsulate a business transaction which can affect the database.</param>
    /// <param name="countryRepository">Represents the repository for <see cref="DomainModel.Country"/>.</param>
    /// <exception cref="ArgumentNullException">When some argument for the constructor parameters is null.</exception>
    public DeleteCountryCommandHandler(
        IMediator mediator,
        IUnitOfWork unitOfWork,
        ICountryRepository countryRepository)
        : base(mediator, unitOfWork)
    {
        _countryRepository = countryRepository ?? throw new ArgumentNullException(nameof(countryRepository));
    }

    #endregion

    #region Public methods

    /// <inheritdoc />
    public override async Task Handle(DeleteCountryCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        CountryService countryService = new (_countryRepository);

        // Gets the current Country.
        CountryIataCode countryIataCode = CountryIataCode.Build(command.IataCode);
        DomainModel.Country? country = await countryService.GetByIataCodeAsync(countryIataCode);

        // Deletes the Country from domain.
        country!.Delete(country);

        _unitOfWork.BeginTransaction();
        await _countryRepository.DeleteAsync(country);
        _unitOfWork.Commit();

        await PublishDomainEvents(country.PullDomainEvents());
    }

    #endregion
}
