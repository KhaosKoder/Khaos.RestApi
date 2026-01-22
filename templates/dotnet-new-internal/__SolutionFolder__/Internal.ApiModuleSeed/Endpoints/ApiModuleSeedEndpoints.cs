using Domain.ApiModuleSeed.Services;
using Internal.ApiModuleSeed.Requests;
using Internal.ApiModuleSeed.Responses;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Internal.ApiModuleSeed.Endpoints;

/// <summary>
/// Minimal API endpoints wired into Host.Internal.
/// </summary>
public static class ApiModuleSeedEndpoints
{
    public static IEndpointRouteBuilder MapApiModuleSeedEndpoints(this IEndpointRouteBuilder endpoints)
    {
        ArgumentNullException.ThrowIfNull(endpoints);

        var group = endpoints
            .MapGroup("/apis/api-module-seed")
            .WithTags("ApiModuleSeed")
            .WithOpenApi();

        group.MapPost("/ping", HandlePingAsync)
            .WithName("ApiModuleSeed_Ping")
            .Produces<PingResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status502BadGateway);

        return endpoints;
    }

    private static async Task<IResult> HandlePingAsync(
        PingRequest request,
        IApiModuleSeedService service,
        CancellationToken cancellationToken)
    {
        var payload = await service.PingAsync(cancellationToken).ConfigureAwait(false);
        return Results.Ok(new PingResponse(payload));
    }
}
