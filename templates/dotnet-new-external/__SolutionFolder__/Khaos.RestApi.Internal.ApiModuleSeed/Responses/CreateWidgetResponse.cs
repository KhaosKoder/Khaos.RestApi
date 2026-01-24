using Khaos.RestApi.Domain.ApiModuleSeed.Models;

namespace Khaos.RestApi.Internal.ApiModuleSeed.Responses;

/// <summary>
/// HTTP response envelope returned to clients.
/// </summary>
public sealed record CreateWidgetResponse(string Id, string Name, string? Status, string? CorrelationId)
{
    public static CreateWidgetResponse From(WidgetResult result)
        => new(result.Id, result.Name, result.Status, result.CorrelationId);
}
