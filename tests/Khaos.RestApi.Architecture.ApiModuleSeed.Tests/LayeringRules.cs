using Khaos.RestApi.Client.Sample.Services;
using Khaos.RestApi.Domain.Sample.Services;
using NetArchTest.Rules;
using Xunit;
using InternalDi = Khaos.RestApi.Internal.Sample.DependencyInjection.ServiceCollectionExtensions;

namespace Khaos.RestApi.Architecture.ApiModuleSeed.Tests;

public sealed class LayeringRules
{
    [Fact]
    public void DomainDependsOnExternal()
    {
        var result = Types.InAssembly(typeof(ISampleService).Assembly)
            .That().ResideInNamespaceStartingWith("Khaos.RestApi.Domain.Sample")
            .Should().HaveDependencyOn("Khaos.RestApi.External.Sample")
            .GetResult();

        AssertSuccessful(result, "Domain must reference External.");
    }

    [Fact]
    public void InternalDoesNotReferenceExternal()
    {
        var result = Types.InAssembly(typeof(InternalDi).Assembly)
            .Should().NotHaveDependencyOn("Khaos.RestApi.External.Sample")
            .GetResult();

        AssertSuccessful(result, "Internal cannot depend on External.");
    }

    [Fact]
    public void ClientDoesNotReferenceDomainOrExternal()
    {
        var result = Types.InAssembly(typeof(ISampleClient).Assembly)
            .Should().NotHaveDependencyOnAny(new[]
            {
                "Khaos.RestApi.Domain.Sample",
                "Khaos.RestApi.External.Sample"
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
