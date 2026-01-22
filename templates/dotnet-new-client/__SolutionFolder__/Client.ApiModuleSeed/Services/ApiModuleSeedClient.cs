using System.Net.Http.Json;
using Client.ApiModuleSeed.Models;

namespace Client.ApiModuleSeed.Services;

internal sealed class ApiModuleSeedClient : IApiModuleSeedClient
{
    private readonly HttpClient _httpClient;

    public ApiModuleSeedClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<PingResult> PingAsync(CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync(
            requestUri: "/apis/api-module-seed/ping",
            value: new { message = "ping" },
            cancellationToken: cancellationToken).ConfigureAwait(false);

        response.EnsureSuccessStatusCode();

        var payload = await response.Content
            .ReadFromJsonAsync<PingResult>(cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        if (payload is null)
        {
            throw new InvalidOperationException("Internal API returned an empty payload.");
        }

        return payload;
    }
}
