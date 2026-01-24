using Khaos.RestApi.Domain.ApiModuleSeed.Options;
using Khaos.RestApi.Domain.ApiModuleSeed.Services;
using Khaos.RestApi.External.ApiModuleSeed;
using Khaos.RestApi.External.ApiModuleSeed.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Khaos.RestApi.Domain.ApiModuleSeed.DependencyInjection;

/// <summary>
/// Registers Domain services plus the dependent External client.
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiModuleSeedDomain(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        services.AddApiModuleSeedExternal(configuration);

        services.AddOptions<ApiModuleSeedDomainOptions>(ApiModuleSeedConstants.ApiName)
            .Bind(configuration.GetSection(ApiModuleSeedDomainOptions.ConfigurationSection))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddScoped<IApiModuleSeedService, ApiModuleSeedService>();
        return services;
    }
}
