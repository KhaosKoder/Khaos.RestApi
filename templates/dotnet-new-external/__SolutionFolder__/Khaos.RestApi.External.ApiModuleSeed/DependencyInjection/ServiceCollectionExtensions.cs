using Common.Abstractions.DependencyInjection;
using Common.Abstractions.Options;
using Khaos.RestApi.External.ApiModuleSeed.Handlers;
using Khaos.RestApi.External.ApiModuleSeed.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Refit;

namespace Khaos.RestApi.External.ApiModuleSeed.DependencyInjection;

/// <summary>
/// Registers the transport-only Refit client for this provider.
/// </summary>
internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiModuleSeedExternal(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        services.AddExternalApiOptions(
            configuration,
            ApiModuleSeedConstants.ApiName,
            sectionPrefix: "Apis");

        services.AddOptions<ApiModuleSeedAuthenticationOptions>(ApiModuleSeedConstants.ApiName)
            .Bind(configuration.GetSection(ApiModuleSeedConstants.ConfigurationSection))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddTransient<ApiAuthenticationDelegatingHandler>();
        services.AddTransient<ApiCallHooksDelegatingHandler>();

        services
            .AddRefitClient<IApiModuleSeedApi>()
            .ConfigureHttpClient((sp, client) =>
            {
                var options = sp
                    .GetRequiredService<IOptionsMonitor<ExternalApiOptions>>()
                    .Get(ApiModuleSeedConstants.ApiName);

                client.BaseAddress = new Uri(options.BaseUrl);
                client.Timeout = options.Timeout;
                client.DefaultRequestHeaders.UserAgent.ParseAdd(options.UserAgent);
            })
            .AddHttpMessageHandler<ApiAuthenticationDelegatingHandler>()
            .AddHttpMessageHandler<ApiCallHooksDelegatingHandler>();

        return services;
    }
}
