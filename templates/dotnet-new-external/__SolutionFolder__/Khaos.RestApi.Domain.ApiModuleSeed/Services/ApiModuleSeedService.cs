using System.Text.Json;
using Common.Abstractions.Time;
using Common.Observability.Tracing;
using Common.Persistence.Repositories;
using Khaos.RestApi.Domain.ApiModuleSeed.Auditing;
using Khaos.RestApi.Domain.ApiModuleSeed.Exceptions;
using Khaos.RestApi.Domain.ApiModuleSeed.Models;
using Khaos.RestApi.Domain.ApiModuleSeed.Options;
using Khaos.RestApi.External.ApiModuleSeed;
using Khaos.RestApi.External.ApiModuleSeed.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Refit;

namespace Khaos.RestApi.Domain.ApiModuleSeed.Services;

/// <summary>
/// Contains the orchestration logic for this API module.
/// </summary>
internal sealed class ApiModuleSeedService : IApiModuleSeedService
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);

    private readonly IApiModuleSeedApi _api;
    private readonly IApiAuditRepository _auditRepository;
    private readonly IUtcClock _clock;
    private readonly ILogger<ApiModuleSeedService> _logger;
    private readonly IOptionsMonitor<ApiModuleSeedDomainOptions> _options;
    private readonly ITracer _tracer;

    public ApiModuleSeedService(
        IApiModuleSeedApi api,
        IApiAuditRepository auditRepository,
        IUtcClock clock,
        ILogger<ApiModuleSeedService> logger,
        IOptionsMonitor<ApiModuleSeedDomainOptions> options,
        ITracer tracer)
    {
        _api = api;
        _auditRepository = auditRepository;
        _clock = clock;
        _logger = logger;
        _options = options;
        _tracer = tracer;
    }

    public async Task<WidgetResult> CreateWidgetAsync(CreateWidgetCommand command, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        using var span = _tracer.StartSpan("Khaos.RestApi.Domain.ApiModuleSeed.CreateWidget");
        var startedAt = _clock.UtcNow;
        var request = new CreateWidgetRequest
        {
            Name = command.Name,
            Description = command.Description
        };

        var response = await _api.CreateWidgetAsync(request, cancellationToken).ConfigureAwait(false);
        var requestJson = Serialize(request) ?? string.Empty;
        var responseJson = Serialize(response.Content) ?? response.Error?.Content;

        if (!response.IsSuccessStatusCode || response.Content is null)
        {
            throw ApiModuleSeedExternalException.FromResponse(
                "CreateWidget",
                response,
                requestJson,
                responseJson,
                span.CorrelationId);
        }

        var requestMessage = response.RequestMessage ?? response.Error?.RequestMessage;

        var httpMethod = requestMessage?.Method.Method ?? "POST";
        var requestPath = requestMessage?.RequestUri?.AbsolutePath ?? "/widgets";

        await TryAuditAsync(
            operation: "CreateWidget",
            httpMethod: httpMethod,
            requestPath: requestPath,
            statusCode: (int)response.StatusCode,
            callerSystem: command.CallerSystem,
            requestJson,
            responseJson,
            span.CorrelationId,
            startedAt,
            _clock.UtcNow,
            cancellationToken).ConfigureAwait(false);

        return new WidgetResult(
            response.Content.Id,
            response.Content.Name,
            response.Content.Status,
            span.CorrelationId);
    }

    private async Task TryAuditAsync(
        string operation,
        string httpMethod,
        string? requestPath,
        int statusCode,
        string? callerSystem,
        string requestJson,
        string? responseJson,
        string? correlationId,
        DateTimeOffset requestTimestamp,
        DateTimeOffset responseTimestamp,
        CancellationToken cancellationToken)
    {
        var options = _options.Get(ApiModuleSeedConstants.ApiName);
        if (!options.EnableAuditing)
        {
            return;
        }

        var audit = ApiModuleSeedAuditRecordFactory.Create(
            operation,
            httpMethod,
            callerSystem,
            statusCode,
            requestPath,
            correlationId,
            requestJson,
            responseJson,
            requestTimestamp,
            responseTimestamp,
            options);

        try
        {
            await _auditRepository.SaveAsync(audit, cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(
                ex,
                "Failed to persist audit entry for {ApiName} operation {Operation}.",
                ApiModuleSeedConstants.ApiName,
                operation);
        }
    }

    private static string? Serialize<T>(T? value)
    {
        return value is null ? null : JsonSerializer.Serialize(value, SerializerOptions);
    }
}
