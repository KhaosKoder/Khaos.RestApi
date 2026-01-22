using Domain.ApiModuleSeed.Exceptions;
using Domain.ApiModuleSeed.Models;
using Domain.ApiModuleSeed.Services;
using Internal.ApiModuleSeed.Requests;
using Internal.ApiModuleSeed.Responses;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OpenApi;
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

        group.MapPost("/widgets", HandleCreateWidgetAsync)
            .WithName("ApiModuleSeed_CreateWidget")
            .Produces<CreateWidgetResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status502BadGateway);

        return endpoints;
    }

    private static async Task<IResult> HandleCreateWidgetAsync(
        CreateWidgetRequest request,
        IApiModuleSeedService service,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = new CreateWidgetCommand
            {
                Name = request.Name,
                Description = request.Description,
                CallerSystem = request.CallerSystem
            };

            var result = await service.CreateWidgetAsync(command, cancellationToken).ConfigureAwait(false);
            var response = CreateWidgetResponse.From(result);

            return Results.Created($"/apis/api-module-seed/widgets/{result.Id}", response);
        }
        catch (ApiModuleSeedExternalException ex)
        {
            var problem = new ProblemDetails
            {
                Title = "Upstream provider rejected the request.",
                Detail = ex.Message,
                Status = StatusCodes.Status502BadGateway,
                Extensions =
                {
                    ["correlationId"] = ex.Data["CorrelationId"] ?? string.Empty,
                    ["providerStatusCode"] = ex.Data["StatusCode"] ?? string.Empty,
                    ["operation"] = ex.Data["operation"] ?? "CreateWidget"
                }
            };

            return Results.Problem(problem);
        }
    }
}
