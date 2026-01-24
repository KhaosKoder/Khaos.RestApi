using System.Globalization;
using Common.Persistence.Entities;
using Khaos.RestApi.Domain.ApiModuleSeed.Options;
using Khaos.RestApi.External.ApiModuleSeed;

namespace Khaos.RestApi.Domain.ApiModuleSeed.Auditing;

/// <summary>
/// Builds audit rows aligned with the shared persistence model.
/// </summary>
internal static class ApiModuleSeedAuditRecordFactory
{
    public static ApiCallAudit Create(
        string operation,
        string httpMethod,
        string? callerSystem,
        int statusCode,
        string? requestPath,
        string? correlationId,
        string requestPayload,
        string? responsePayload,
        DateTimeOffset requestTimestamp,
        DateTimeOffset responseTimestamp,
        ApiModuleSeedDomainOptions options)
    {
        return new ApiCallAudit
        {
            ApiName = ApiModuleSeedConstants.ApiName,
            Operation = operation,
            Direction = options.TableMode,
            CallerSystem = callerSystem ?? options.DbSchema,
            StatusCode = statusCode,
            ErrorCode = statusCode >= 400 ? statusCode.ToString(CultureInfo.InvariantCulture) : null,
            ErrorMessage = statusCode >= 400 ? "Provider returned an error." : null,
            HttpMethod = httpMethod,
            RequestPath = requestPath,
            RequestPayload = requestPayload,
            ResponsePayload = responsePayload,
            RequestTimestampUtc = requestTimestamp,
            ResponseTimestampUtc = responseTimestamp,
            DurationMs = (long)(responseTimestamp - requestTimestamp).TotalMilliseconds,
            CorrelationId = correlationId
        };
    }
}
