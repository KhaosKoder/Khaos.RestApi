using Khaos.RestApi.Domain.Sample.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Khaos.RestApi.Internal.Sample.DependencyInjection;

/// <summary>
/// Composes the Internal layer for this module.
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSampleInternal(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        services.AddSampleDomain(configuration);
        return services;
    }
}
