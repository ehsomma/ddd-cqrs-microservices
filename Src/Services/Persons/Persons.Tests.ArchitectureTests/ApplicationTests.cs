#region Usings

using FluentAssertions;
using NetArchTest.Rules;
using Records.Shared.Application.Handlers;
using System.Reflection;
using Xunit;

#endregion

namespace Persons.Tests.ArchitectureTests;

public class ApplicationTests
{
    #region Declarations

    private const string PersonsDomainNamespace = "Records.Persons.Application";

    #endregion

    #region Tests

    [Fact]
    public void command_handlers_should_inherit_from_CommandHandler_base_class()
    {
        // Arrange.
        Types? types = Types.InAssembly(Assembly.Load(PersonsDomainNamespace));

        // Act.
        TestResult? testResult =
            types
                .That()
                .HaveNameEndingWith("CommandHandler", StringComparison.InvariantCulture)
                .Should()
                .Inherit(typeof(CommandHandler<>))
                .GetResult();

        // Assert.
        testResult.IsSuccessful.Should().BeTrue();
    }

    #endregion
}
