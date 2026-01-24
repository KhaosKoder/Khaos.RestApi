using Khaos.RestApi.Client.ApiModuleSeed.Services;
using Common.Abstractions.DependencyInjection;
using Common.Abstractions.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Khaos.RestApi.Client.ApiModuleSeed.DependencyInjection;

/// <summary>
/// Registers the SDK + typed HttpClient.
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiModuleSeedClient(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        services.AddInternalApiOptions(configuration, ApiModuleSeedClientConstants.ApiName, sectionPrefix: "InternalApis");

        services.AddHttpClient<IApiModuleSeedClient, ApiModuleSeedClient>((sp, client) =>
        {
            var options = sp.GetRequiredService<IOptionsMonitor<InternalApiOptions>>()
                .Get(ApiModuleSeedClientConstants.ApiName);

            client.BaseAddress = new Uri(options.BaseUrl);
            client.Timeout = options.Timeout;
            client.DefaultRequestHeaders.UserAgent.ParseAdd($"Khaos.RestApi.Client.{ApiModuleSeedClientConstants.ApiName}/1.0");
        });

        return services;
    }
}
