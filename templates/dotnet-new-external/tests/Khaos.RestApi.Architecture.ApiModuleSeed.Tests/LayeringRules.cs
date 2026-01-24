using Khaos.RestApi.Client.ApiModuleSeed.Services;
using Khaos.RestApi.Domain.ApiModuleSeed.Services;
using NetArchTest.Rules;
using Xunit;
using InternalDi = Khaos.RestApi.Internal.ApiModuleSeed.DependencyInjection.ServiceCollectionExtensions;

namespace Khaos.RestApi.Architecture.ApiModuleSeed.Tests;

public sealed class LayeringRules
{
    [Fact]
    public void DomainDependsOnExternal()
    {
        var result = Types.InAssembly(typeof(IApiModuleSeedService).Assembly)
            .That().AreClasses()
            .And().ResideInNamespace("Khaos.RestApi.Domain.ApiModuleSeed.Services")
            .Should().HaveDependencyOn("Khaos.RestApi.External.ApiModuleSeed")
            .GetResult();

        AssertSuccessful(result, "Domain must reference External.");
    }

    [Fact]
    public void InternalDoesNotReferenceExternal()
    {
        var result = Types.InAssembly(typeof(InternalDi).Assembly)
            .Should().NotHaveDependencyOn("Khaos.RestApi.External.ApiModuleSeed")
            .GetResult();

        AssertSuccessful(result, "Internal cannot depend on External.");
    }

    [Fact]
    public void ClientDoesNotReferenceDomainOrExternal()
    {
        var result = Types.InAssembly(typeof(IApiModuleSeedClient).Assembly)
            .Should().NotHaveDependencyOnAny(new[]
            {
                "Khaos.RestApi.Domain.ApiModuleSeed",
                "Khaos.RestApi.External.ApiModuleSeed"
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
