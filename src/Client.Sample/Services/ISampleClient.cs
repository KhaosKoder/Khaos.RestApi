using Client.Sample.Models;

namespace Client.Sample.Services;

/// <summary>
/// Contract exposed to downstream callers.
/// </summary>
public interface ISampleClient
{
    Task<CreateWidgetResult> CreateWidgetAsync(CreateWidgetRequest request, CancellationToken cancellationToken = default);
}
