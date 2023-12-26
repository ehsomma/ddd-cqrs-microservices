#region Usings

using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Options;
using Moq;
using Records.Persons.Application.Persons.Commands.CreatePerson;
using Records.Persons.Domain.Persons.Repositories;
using Records.Shared.Domain.Exceptions;
using Records.Shared.Infra.Persistence.Abstractions;
using Xunit;
using CountriesErrors = Records.Persons.Domain.Countries.Errors; // Using aliases.
using DomainModel = Records.Persons.Domain.Persons.Models; // Using aliases.
using DomainServices = Records.Persons.Domain.Countries.Services; // Using aliases.
////using PersonsErrors = Records.Persons.Domain.Persons.Errors; // Using aliases.
using Dto = Records.Persons.Dtos.Persons; // Using aliases.
using DtoShared = Records.Persons.Dtos.Shared; // Using aliases.

#endregion

namespace Persons.Tests.UnitTests.Persons.Commands;

public class CreatePersonCommandHandlerTests : IDisposable
{
    #region Declarations

    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IOptionsSnapshot<DtoShared.PersonsSettings>> _personSettingsMock;
    private readonly Mock<IPersonRepository> _personRepositoryMock;
    private readonly Mock<DomainServices.ICountryService> _countryServiceMock;

    #endregion

    #region Contructor

    public CreatePersonCommandHandlerTests()
    {
        DtoShared.PersonsSettings options = new ()
        {
            Setting1 = "1",
            Setting2 = "2",
        };

        _mediatorMock = new Mock<IMediator>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _personSettingsMock = new Mock<IOptionsSnapshot<DtoShared.PersonsSettings>>();
        _personRepositoryMock = new Mock<IPersonRepository>();
        _countryServiceMock = new Mock<DomainServices.ICountryService>();

        _personSettingsMock.Setup(m => m.Value).Returns(options);
    }

    #endregion

    #region Tests

    ////[Fact(Skip = "Ignores the test")]
    [Fact]
    public async Task handle_should_call_repository_InsertAsync_when_all_the_data_is_ok()
    {
        // Arrange.
        CreatePersonCommand command = BuildCommand_Ok();
        ////CancellationToken cancellationToken = new CancellationTokenSource().Token;

        // MediatR, UnitOfWork, IOptionSnapshot, IPersonRepository, CountryService.
        CreatePersonCommandHandler handler = new (
            _mediatorMock.Object,
            _unitOfWorkMock.Object,
            _personSettingsMock.Object,
            _personRepositoryMock.Object,
            _countryServiceMock.Object);

        _countryServiceMock.Setup(x => x.EnsureExists(
            It.IsAny<DomainModel.Address>())); // .Throw...

        // Act.
        await handler.Handle(command, default);

        // Assert.
        _personRepositoryMock.Verify(
            x => x.InsertAsync(It.IsAny<DomainModel.Person>()),
            Times.Once);

        // It.IsAny vs It.Is. (It.Is allow as validate some property).
        ////_personRepositoryMock.Verify(
        ////    x => x.InsertAsync(It.Is<DomainModel.Person>(p => p.Id == command.Person.Id)),
        ////    Times.Once);
    }

    [Fact]
    public async Task handle_should_throw_DomainException_with_ERR_COUNTRY_NOTFOUND_when_the_country_not_found()
    {
        // Arrange.
        CreatePersonCommand command = BuildCommand_WithNonexistentCountry();

        CreatePersonCommandHandler handler = new (
            _mediatorMock.Object,
            _unitOfWorkMock.Object,
            _personSettingsMock.Object,
            _personRepositoryMock.Object,
            _countryServiceMock.Object);

        _countryServiceMock.Setup(x => x.EnsureExists(
                It.Is<DomainModel.Address>(a => a.Country! == "Nonexistent country")))
            .Throws(new DomainException(CountriesErrors.DomainErrors.Country.NotFound));

        // Act.
        Func<Task> action = async () =>
        {
            await handler.Handle(command, default);
        };

        // Assert.
        await action.Should()
            .ThrowAsync<DomainException>()
            .Where(ex => ex.Error.Code == CountriesErrors.DomainErrors.Country.NotFound.Code);
    }

    #endregion

    #region IDisposable

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            // Free managed resources.
        }

        // Free native resources if there are any.
    }

    #endregion

    #region Private methods

    /// <summary>
    /// Creates a CreatePersonCommand with full data.
    /// </summary>
    private CreatePersonCommand BuildCommand_Ok()
    {
        string apyKey = string.Empty;

        Dto.Address address = new ()
        {
            StreetLine1 = "4441 Collins Avenue",
            StreetLine2 = "-",
            City = "Miami Beach",
            State = "FL",
            Country = "United States",
            LatLng = new Dto.LatLng()
            {
                Lat = (decimal?)25.817887,
                Lng = (decimal?)-80.122785D,
            },
        };

        IList<Dto.PersonalAsset> personalAssets = new List<Dto.PersonalAsset>()
        {
            new Dto.PersonalAsset()
            {
                Description = "Oculus Quest 2",
                Value = 400,
            },
        };

        Dto.Person person = new ()
        {
            Id = Guid.NewGuid(),
            FullName = "Doe, John",
            Email = "johndoe@gmail.com",
            Phone = "123456789",
            Gender = "Male",
            Birthdate = null,
            Address = address,
            PersonalAssets = personalAssets,
        };

        CreatePersonCommand command = new (apyKey, person);

        return command;
    }

    /// <summary>
    /// Creates a CreatePersonCommand with duplicated country.
    /// </summary>
    private CreatePersonCommand BuildCommand_WithNonexistentCountry()
    {
        string apyKey = string.Empty;

        Dto.Address address = new ()
        {
            StreetLine1 = "4441 Collins Avenue",
            StreetLine2 = "-",
            City = "Miami Beach",
            State = "FL",
            Country = "Nonexistent country", // <==.
            LatLng = new Dto.LatLng()
            {
                Lat = (decimal?)25.817887,
                Lng = (decimal?)-80.122785D,
            },
        };

        IList<Dto.PersonalAsset> personalAssets = new List<Dto.PersonalAsset>()
        {
            new Dto.PersonalAsset()
            {
                Description = "Oculus Quest 2",
                Value = 400,
            },
        };

        Dto.Person person = new ()
        {
            Id = Guid.NewGuid(),
            FullName = "Doe, John",
            Email = "johndoe@gmail.com",
            Phone = "123456789",
            Gender = "Male",
            Birthdate = null,
            Address = address,
            PersonalAssets = personalAssets,
        };

        CreatePersonCommand command = new (apyKey, person);

        return command;
    }

    #endregion
}
