using Khaos.RestApi.Client.ApiModuleSeed.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Khaos.RestApi.Client.ApiModuleSeed.DependencyInjection;

/// <summary>
/// Registers the HTTP client for the Internal host endpoints.
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiModuleSeedClient(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        services.AddHttpClient<IApiModuleSeedClient, ApiModuleSeedClient>((sp, client) =>
        {
            var baseUrl = configuration[$"Clients:{ApiModuleSeedClientOptions.ConfigurationSection}:BaseUrl"] ?? "https://localhost";
            client.BaseAddress = new Uri(baseUrl);
        });

        return services;
    }
}
