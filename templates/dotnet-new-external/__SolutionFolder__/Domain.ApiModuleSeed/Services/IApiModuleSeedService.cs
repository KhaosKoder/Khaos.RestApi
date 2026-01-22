using Domain.ApiModuleSeed.Models;

namespace Domain.ApiModuleSeed.Services;

/// <summary>
/// Domain entry-point for orchestration logic.
/// </summary>
public interface IApiModuleSeedService
{
    Task<WidgetResult> CreateWidgetAsync(CreateWidgetCommand command, CancellationToken cancellationToken = default);
}
