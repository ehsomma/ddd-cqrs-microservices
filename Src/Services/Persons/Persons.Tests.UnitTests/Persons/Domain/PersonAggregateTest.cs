#region Usings

using FluentAssertions;
using Records.Persons.Domain.Persons.Enumerators;
using Records.Persons.Domain.Persons.Models;
using Records.Persons.Domain.Persons.ValueObjects;
using Records.Persons.Domain.Shared.ValueObjects;
using Records.Shared.Domain.Exceptions;
using Records.Shared.Domain.ValueObjects;
using Xunit;

#endregion

namespace Persons.Tests.UnitTests.Persons.Domain;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0059:Unnecessary assignment of a value", Justification = "Not for tests.")]
public class PersonAggregateTest : IDisposable
{
    #region Contructor

    public PersonAggregateTest()
    {
    }

    #endregion

    #region Tests

    [Fact]
    public void address_should_be_created_with_valid_data()
    {
        // Arrange.
        Address address = null!;

        // Act.
        address = Address.Create(
            StreetLine.Build("4441 Collins Avenue"),
            StreetLine2.Build(null),
            City.Build("Miami Beach"),
            State.Build("FL"),
            CountryName.Build("United States"),
            LatLng.Build(25.817887M, -80.122785M));

        // Assert.
        address.Should().NotBe(null);
    }

    [Fact]
    public void should_throw_DomainValidationException_if_attempt_to_build_an_address_without_streetline()
    {
        // Arrange.

        // Act.
        Action action = () =>
        {
            _ = Address.Create(
                StreetLine.Build(null), // <===.
                StreetLine2.Build(null),
                City.Build("Miami Beach"),
                State.Build("FL"),
                CountryName.Build("United States"),
                LatLng.Build(25.817887M, -80.122785M));
        };

        // Assert.
        action.Should().Throw<DomainValidationException>()
            .WithMessage("Value cannot be null. (Parameter 'StreetLine')");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(4)]
    public void gender_should_throw_exception_if_attempt_to_get_an_item_from_an_invalid_value(int genderValue)
    {
        // Arrange.

        // Act.
        Action action = () =>
        {
            Gender invalidGender = Gender.FromValue(genderValue);
        };

        // Assert.
        action.Should().Throw<DomainValidationException>()
            .And.Message.Should().Contain("Possible values for Gender: ");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("1")]
    [InlineData("Martian")]
    [InlineData("male")] // In lowercase.
    public void gender_should_throw_exception_if_attempt_to_get_an_item_from_an_invalid_name(string? gender)
    {
        // Arrange.

        // Act.
        Action action = () =>
        {
            Gender invalidGender = Gender.FromName(gender);
        };

        // Assert.
        action.Should().Throw<DomainValidationException>()
            .And.Message.Should().Contain("Possible values for Gender: ");
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
}
