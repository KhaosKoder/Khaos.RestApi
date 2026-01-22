using Client.ApiModuleSeed.Models;

namespace Client.ApiModuleSeed.Services;

/// <summary>
/// Contract exposed to downstream callers.
/// </summary>
public interface IApiModuleSeedClient
{
    Task<CreateWidgetResult> CreateWidgetAsync(CreateWidgetRequest request, CancellationToken cancellationToken = default);
}
