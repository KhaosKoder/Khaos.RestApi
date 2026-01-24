using Khaos.RestApi.Domain.Sample.Options;
using Khaos.RestApi.Domain.Sample.Services;
using Khaos.RestApi.External.Sample;
using Khaos.RestApi.External.Sample.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Khaos.RestApi.Domain.Sample.DependencyInjection;

/// <summary>
/// Registers Domain services plus the dependent External client.
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSampleDomain(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        services.AddSampleExternal(configuration);

        services.AddOptions<SampleDomainOptions>(SampleConstants.ApiName)
            .Bind(configuration.GetSection(SampleDomainOptions.ConfigurationSection))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddScoped<ISampleService, SampleService>();
        return services;
    }
}
