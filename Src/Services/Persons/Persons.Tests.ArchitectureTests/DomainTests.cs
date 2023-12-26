#region Usings

using FluentAssertions;
using NetArchTest.Rules;
using Records.Shared.Domain.Events;
using Records.Shared.Domain.Models;
using System.Reflection;
using Xunit;

#endregion

namespace Persons.Tests.ArchitectureTests;

public class DomainTests
{
    #region Declarations

    private const string PersonsDomainNamespace = "Records.Persons.Domain";

    #endregion

    #region Tests

    [Fact]
    public void domain_public_classes_should_be_sealed()
    {
        // Why: https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/abstract-and-sealed-classes-and-class-members.

        // Arrange.
        Types? types = Types.InAssembly(Assembly.Load(PersonsDomainNamespace));

        // Act.
        TestResult? testResult =
            types
                .That()
                .AreNotInterfaces()
                .And()
                .ArePublic()
                .Should()
                .BeSealed()
                .GetResult();

        // Assert.
        testResult.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void domain_public_classes_should_be_inmutable()
    {
        // Arrange.
        Types? types = Types.InAssembly(Assembly.Load(PersonsDomainNamespace));

        // Act.
        TestResult? testResult =
            types
                .That()
                .ArePublic()
                .Should()
                .BeImmutable()
                .GetResult();

        // Assert.
        testResult.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void domain_entities_should_have_not_public_constructor()
    {
        // Arrange.
        IEnumerable<Type>? entitiesTypes =
            Types
                .InAssembly(Assembly.Load(PersonsDomainNamespace))
                .That()
                .Inherit(typeof(Entity<>))
                .GetTypes(); // It is different than other tests (gets an IEnumerable<Type>?)!

        List<Type> failingTypes = new ();

        // Act.
        foreach (Type entitiesType in entitiesTypes)
        {
            ConstructorInfo[] constructor = entitiesType.GetConstructors(
                BindingFlags.Public | BindingFlags.Instance);

            if (constructor.Length > 0)
            {
                failingTypes.Add(entitiesType);
            }
        }

        // Assert.
        failingTypes.Should().BeEmpty();
    }

    [Fact]
    public void domain_events_should_end_with_event()
    {
        // Arrange.
        Types? types = Types.InAssembly(Assembly.Load(PersonsDomainNamespace));

        // Act.
        TestResult? testResult =
            types
                .That()
                .ImplementInterface(typeof(IDomainEvent))
                .Should()
                .HaveNameEndingWith("Event", StringComparison.InvariantCulture)
                .GetResult();

        // Assert.
        testResult.IsSuccessful.Should().BeTrue();
    }

    #endregion
}
