using Domain.ApiModuleSeed.Options;
using Domain.ApiModuleSeed.Services;
using External.ApiModuleSeed;
using External.ApiModuleSeed.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Domain.ApiModuleSeed.DependencyInjection;

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
