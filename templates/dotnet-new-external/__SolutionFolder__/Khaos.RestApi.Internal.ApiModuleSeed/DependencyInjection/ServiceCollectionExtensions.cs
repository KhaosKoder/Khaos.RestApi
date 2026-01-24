using Khaos.RestApi.Domain.ApiModuleSeed.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Khaos.RestApi.Internal.ApiModuleSeed.DependencyInjection;

/// <summary>
/// Composes the Internal layer for this module.
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiModuleSeedInternal(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        services.AddApiModuleSeedDomain(configuration);
        return services;
    }
}
