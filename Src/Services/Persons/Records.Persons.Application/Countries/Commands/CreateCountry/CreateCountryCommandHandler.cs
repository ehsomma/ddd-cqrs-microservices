#region Usings

using MediatR;
using Records.Persons.Domain.Countries.Repositories;
using Records.Persons.Domain.Countries.ValueObjects;
using Records.Persons.Domain.Shared.ValueObjects;
using Records.Shared.Application.Handlers;
using Records.Shared.Infra.Persistence.Abstractions;
using DomainModel = Records.Persons.Domain.Countries.Models; // Using aliases.

#endregion

namespace Records.Persons.Application.Countries.Commands.CreateCountry;

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
        DomainModel.Country country = CreateCountry(command);

        _unitOfWork.BeginTransaction();
        await _countryRepository.InsertAsync(country);
        _unitOfWork.Commit();

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
        ArgumentNullException.ThrowIfNull(command);

        CountryIataCode iataCode = CountryIataCode.Build(command.Country.IataCode);
        CountryName name = CountryName.Build(command.Country.Name);

        DomainModel.Country domainCountry = DomainModel.Country.Create(iataCode, name);

        return domainCountry;
    }

    #endregion
}
