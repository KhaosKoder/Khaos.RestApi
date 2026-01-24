using Khaos.RestApi.Client.ApiModuleSeed.Models;

namespace Khaos.RestApi.Client.ApiModuleSeed.Services;

/// <summary>
/// Contract exposed to downstream callers.
/// </summary>
public interface IApiModuleSeedClient
{
    Task<CreateWidgetResult> CreateWidgetAsync(CreateWidgetRequest request, CancellationToken cancellationToken = default);
}
