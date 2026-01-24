using System.Net.Http.Json;
using Khaos.RestApi.Client.ApiModuleSeed.Exceptions;
using Khaos.RestApi.Client.ApiModuleSeed.Models;

namespace Khaos.RestApi.Client.ApiModuleSeed.Services;

/// <summary>
/// HTTP client that targets Host.Internal endpoints for this module.
/// </summary>
internal sealed class ApiModuleSeedClient : IApiModuleSeedClient
{
    private readonly HttpClient _httpClient;

    public ApiModuleSeedClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<CreateWidgetResult> CreateWidgetAsync(
        CreateWidgetRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var response = await _httpClient.PostAsJsonAsync(
            requestUri: ApiModuleSeedClientConstants.EndpointPrefix + "/widgets",
            value: request,
            cancellationToken: cancellationToken).ConfigureAwait(false);

        if (response.IsSuccessStatusCode)
        {
            var payload = await response.Content
                .ReadFromJsonAsync<CreateWidgetResult>(cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            if (payload is null)
            {
                throw new InvalidOperationException("Internal API returned an empty payload.");
            }

            return payload;
        }

        var problem = await response.Content
            .ReadFromJsonAsync<ProblemDetailsPayload>(cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        throw InternalApiException.FromProblem(response.StatusCode, problem);
    }
}
