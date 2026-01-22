using Common.Abstractions.Time;
using Common.Observability.Tracing;

namespace Domain.ApiModuleSeed.Services;

/// <summary>
/// Minimal domain service placeholder.
/// </summary>
internal sealed class ApiModuleSeedService : IApiModuleSeedService
{
    private readonly IUtcClock _clock;
    private readonly ITracer _tracer;

    public ApiModuleSeedService(IUtcClock clock, ITracer tracer)
    {
        _clock = clock;
        _tracer = tracer;
    }

    public Task<string> PingAsync(CancellationToken cancellationToken = default)
    {
        using var span = _tracer.StartSpan("Domain.ApiModuleSeed.Ping");
        return Task.FromResult($"pong:{_clock.UtcNow:O}:{span.CorrelationId}");
    }
}
