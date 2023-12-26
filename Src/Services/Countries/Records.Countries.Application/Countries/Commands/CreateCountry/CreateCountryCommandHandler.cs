#region Usings

using MediatR;
using Records.Countries.Domain.Countries.Repositories;
using Records.Countries.Domain.Countries.ValueObjects;
using Records.Shared.Application.Handlers;
using Records.Shared.Infra.Persistence.Abstractions;
using DomainModel = Records.Countries.Domain.Countries.Models; // Using aliases.
using Dto = Records.Countries.Dtos.Countries; // Using aliases.

#endregion

namespace Records.Countries.Application.Countries.Commands.CreateCountry;

/// <summary>
/// Represents the <see cref="CreateCountryCommand"/> handler.
/// </summary>
internal sealed class CreateCountryCommandHandler : CommandHandler<CreateCountryCommand>
{
    #region Declarations

    /// <summary>Represents the repository for <see cref="DomainModel.Country"/>.</summary>
    private readonly ICountryRepository _countryRepository;

    #endregion

    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateCountryCommandHandler"/> class.
    /// </summary>
    /// <param name="mediator">MediatR library to send and handle commands and queries implementing CQRS.</param>
    /// <param name="unitOfWork">Manage a <see cref="IDbSession"/> to encapsulate a business transaction which can affect the database.</param>
    /// <param name="countryRepository">Represents the repository for <see cref="DomainModel.Country"/>.</param>
    /// <exception cref="ArgumentNullException">When some argument for the constructor parameters is null.</exception>
    public CreateCountryCommandHandler(
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
    public override async Task Handle(CreateCountryCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        DomainModel.Country country = CreateCountry(command);

        ////_unitOfWork.BeginTransaction();
        ////await _countryRepository.InsertAsync(country);
        ////_unitOfWork.Commit();

        await PublishDomainEvents(country.PullDomainEvents());
    }

    #endregion

    #region Private methods

    /// <summary>
    /// Create a new <seealso cref="DomainModel.Country"/> with the the data of the specified <paramref name="command"/>.
    /// </summary>
    /// <param name="command">The command with the data to create a new country.</param>
    /// <returns>A new <seealso cref="DomainModel.Country"/>.</returns>
    private DomainModel.Country CreateCountry(CreateCountryCommand command)
    {
        ArgumentNullException.ThrowIfNull(nameof(command));

        Dto.Country countryDto = command.Country;

        CountryIataCode countryIataCode = CountryIataCode.Build(countryDto.IataCode);
        CountryName countryName = CountryName.Build(countryDto.Name);
        CountryLanguages countryLanguages = CountryLanguages.Build(countryDto.Languages);
        CountryCurrency countryCurrency = CountryCurrency.Build(countryDto.Currency);
        CountryCapital countryCapital = CountryCapital.Build(countryDto.Capital);
        CountryNeighbors countryNeighbors = CountryNeighbors.Build(countryDto.Neighbors);
        CountryFlagUrl countryFlagUrl = CountryFlagUrl.Build(countryDto.FlagUrl);

        DomainModel.Country domainCountry = DomainModel.Country.Create(
            countryIataCode,
            countryName,
            countryLanguages,
            countryCurrency,
            countryCapital,
            countryDto.Area,
            countryDto.Population,
            countryNeighbors,
            countryFlagUrl);

        return domainCountry;
    }

    #endregion
}
