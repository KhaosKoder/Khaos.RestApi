using System.Net.Http.Headers;
using External.ApiModuleSeed.Options;
using Microsoft.Extensions.Options;

namespace External.ApiModuleSeed.Handlers;

/// <summary>
/// Applies the configured authentication scheme to outbound HTTP requests.
/// </summary>
internal sealed class ApiAuthenticationDelegatingHandler : DelegatingHandler
{
    private readonly IOptionsMonitor<ApiModuleSeedAuthenticationOptions> _options;

    public ApiAuthenticationDelegatingHandler(IOptionsMonitor<ApiModuleSeedAuthenticationOptions> options)
    {
        _options = options;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var authOptions = _options.Get(ApiModuleSeedConstants.ApiName);
        if (!string.IsNullOrWhiteSpace(authOptions.BearerToken))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue(
                authOptions.Scheme,
                authOptions.BearerToken);
        }

        return base.SendAsync(request, cancellationToken);
    }
}
