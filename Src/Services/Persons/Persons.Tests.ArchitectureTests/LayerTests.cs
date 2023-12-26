#region Usings

using FluentAssertions;
using NetArchTest.Rules;
using System.Reflection;
using Xunit;

#endregion

namespace Persons.Tests.ArchitectureTests;

public class LayerTests
{
    #region Declarations

    ////private const string SharedDomainNamespace = "Records.Shared.Domain";
    private const string PersonsDomainNamespace = "Records.Persons.Domain";
    private const string PersonsApplicationNamespace = "Records.Persons.Application";
    private const string PersonsInfrastructurePersistenceNamespace = "Records.Persons.Infra.Persistence";
    private const string PersonsInfrastructureProjectionNamespace = "Records.Persons.Infra.Projection";
    private const string PersonsInfrastructureReadsNamespace = "Records.Persons.Infra.Reads";
    private const string PersonsApiNamespace = "Records.Persons.Api.V1";
    private const string CountriesApiNamespace = "Records.Countries.Api.V1";
    ////private const string PersonsProjectionsNamespace = "Records.Persons.Projection";
    private const string PersonsBackgroundTasksNamespace = "Records.Persons.BackgroundTasks";
    private const string CountriesBackgroundTasksNamespace = "Records.Countries.BackgroundTasks";

    #endregion

    #region Tests

    [Fact]
    public void domain_should_not_have_dependencies_on_others_projects()
    {
        // Arrange.
        ////Assembly assembly = typeof(Records.Persons.Domain.Persons.Models.Person).Assembly;
        Types? types = Types.InAssembly(Assembly.Load(PersonsDomainNamespace));

        string[] otherProjects = new[]
        {
            PersonsApplicationNamespace,
            PersonsInfrastructurePersistenceNamespace,
            PersonsInfrastructureProjectionNamespace,
            PersonsInfrastructureReadsNamespace,
            PersonsApiNamespace,
        };

        // Act.
        TestResult? testResult =
            types
                .ShouldNot()
                .HaveDependencyOnAny(otherProjects)
                .GetResult();

        // Assert.
        testResult.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void application_should_not_have_dependencies_on_any_apps_projects()
    {
        // Arrange.
        Types? types = Types.InAssembly(Assembly.Load(PersonsApplicationNamespace));

        string[] otherProjects = new[]
        {
            PersonsApiNamespace,
            CountriesApiNamespace,
            PersonsBackgroundTasksNamespace,
            CountriesBackgroundTasksNamespace,
        };

        // Act.
        TestResult? testResult =
            types
                .ShouldNot()
                .HaveDependencyOnAny(otherProjects)
                .GetResult();

        // Assert.
        testResult.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void command_handlers_should_have_dependency_on_domain()
    {
        // Arrange.
        Types? types = Types.InAssembly(Assembly.Load(PersonsApplicationNamespace));

        // Act.
        TestResult? testResult =
            types
                .That()
                .HaveNameEndingWith("CommandHandler", StringComparison.InvariantCulture)
                .Should()
                .HaveDependencyOn(PersonsDomainNamespace)
                .GetResult();

        // Assert.
        testResult.IsSuccessful.Should().BeTrue();
    }

    #endregion
}
