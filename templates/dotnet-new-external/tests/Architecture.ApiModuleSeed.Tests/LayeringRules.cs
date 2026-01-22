using Client.ApiModuleSeed.Services;
using Domain.ApiModuleSeed.Services;
using NetArchTest.Rules;
using Xunit;
using InternalDi = Internal.ApiModuleSeed.DependencyInjection.ServiceCollectionExtensions;

namespace Architecture.ApiModuleSeed.Tests;

public sealed class LayeringRules
{
    [Fact]
    public void DomainDependsOnExternal()
    {
        var result = Types.InAssembly(typeof(IApiModuleSeedService).Assembly)
            .That().AreClasses()
            .And().ResideInNamespace("Domain.ApiModuleSeed.Services")
            .Should().HaveDependencyOn("External.ApiModuleSeed")
            .GetResult();

        AssertSuccessful(result, "Domain must reference External.");
    }

    [Fact]
    public void InternalDoesNotReferenceExternal()
    {
        var result = Types.InAssembly(typeof(InternalDi).Assembly)
            .Should().NotHaveDependencyOn("External.ApiModuleSeed")
            .GetResult();

        AssertSuccessful(result, "Internal cannot depend on External.");
    }

    [Fact]
    public void ClientDoesNotReferenceDomainOrExternal()
    {
        var result = Types.InAssembly(typeof(IApiModuleSeedClient).Assembly)
            .Should().NotHaveDependencyOnAny(new[]
            {
                "Domain.ApiModuleSeed",
                "External.ApiModuleSeed"
            })
            .GetResult();

        AssertSuccessful(result, "Client boundary violated.");
    }

    private static void AssertSuccessful(TestResult result, string message)
    {
        if (result.IsSuccessful)
        {
            return;
        }

        var failingTypes = string.Join(", ", result.FailingTypes);
        throw new Xunit.Sdk.XunitException($"{message} Violations: {failingTypes}");
    }
}
