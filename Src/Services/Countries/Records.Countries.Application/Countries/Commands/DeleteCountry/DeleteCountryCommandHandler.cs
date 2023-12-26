#region Usings

using MediatR;
using Records.Countries.Domain.Countries.Repositories;
using Records.Countries.Domain.Countries.ValueObjects;
using Records.Shared.Application.Handlers;
using Records.Shared.Infra.Persistence.Abstractions;
using DomainModel = Records.Countries.Domain.Countries.Models; // Using aliases.

#endregion

namespace Records.Countries.Application.Countries.Commands.DeleteCountry;

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

        ////CountryService countryService = new CountryService(_countryRepository);

        // Gets the current DomainModel.Country.
        ////DomainModel.Country? country = await countryService.GetByIataCodeAsync(command.IataCode);

        // NOTE: We mocked the country to avoid saving it and obtaining it from the Countries microservice
        // database. We just need here in the Countries microservice to fire the CountryCreatedEvent
        // event. The entire demo is based on the Persons microservice.
        DomainModel.Country country = GetMockCountry(command.IataCode, "MockCountry");

        // Deletes the Country in the domain.
        country!.Delete(country);

        ////_unitOfWork.BeginTransaction();
        ////await _countryRepository.DeleteAsync(country);
        ////_unitOfWork.Commit();

        await PublishDomainEvents(country.PullDomainEvents());
    }

    #endregion

    #region Private methods

    /// <summary>
    /// Gets a mocked Country.
    /// </summary>
    /// <param name="iataCode">Country IATA code.</param>
    /// <param name="name">The country name.</param>
    /// <returns>A mocked Country.</returns>
    private DomainModel.Country GetMockCountry(string iataCode, string name)
    {
        ArgumentNullException.ThrowIfNull(nameof(iataCode));
        ArgumentNullException.ThrowIfNull(nameof(name));

        CountryIataCode countryIataCode = CountryIataCode.Build(iataCode);
        CountryName countryName = CountryName.Build(name);
        CountryLanguages countryLanguages = CountryLanguages.Build("--");
        CountryCurrency countryCurrency = CountryCurrency.Build("---");
        CountryCapital countryCapital = CountryCapital.Build(null);
        CountryNeighbors countryNeighbors = CountryNeighbors.Build(null);
        CountryFlagUrl countryFlagUrl = CountryFlagUrl.Build(null);

        DomainModel.Country domainCountry = DomainModel.Country.Load(
            countryIataCode,
            countryName,
            countryLanguages,
            countryCurrency,
            countryCapital,
            0,
            0,
            countryNeighbors,
            countryFlagUrl);

        return domainCountry;
    }

    #endregion
}
