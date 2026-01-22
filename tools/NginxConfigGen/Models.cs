using System.Text.Json.Serialization;

namespace NginxConfigGen;

public sealed class NginxConfigRoot
{
    [JsonPropertyName("apis")]
    public List<ApiConfig> Apis { get; init; } = new();
}

public sealed class ApiConfig
{
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    [JsonPropertyName("pathPrefix")]
    public string PathPrefix { get; init; } = string.Empty;

    [JsonPropertyName("upstream")]
    public UpstreamConfig Upstream { get; init; } = new();

    [JsonPropertyName("rateLimit")]
    public RateLimitConfig RateLimit { get; init; } = new();

    [JsonPropertyName("timeouts")]
    public TimeoutConfig Timeouts { get; init; } = new();

    [JsonPropertyName("methods")]
    public IReadOnlyList<string> Methods { get; init; } = Array.Empty<string>();

    [JsonPropertyName("auth")]
    public AuthConfig Auth { get; init; } = new();
}

public sealed class UpstreamConfig
{
    [JsonPropertyName("serviceName")]
    public string ServiceName { get; init; } = string.Empty;

    [JsonPropertyName("port")]
    public int Port { get; init; }
}

public sealed class RateLimitConfig
{
    [JsonPropertyName("enabled")]
    public bool Enabled { get; init; }

    [JsonPropertyName("requestsPerMinute")]
    public int RequestsPerMinute { get; init; }
}

public sealed class TimeoutConfig
{
    [JsonPropertyName("connectSeconds")]
    public int ConnectSeconds { get; init; }

    [JsonPropertyName("sendSeconds")]
    public int SendSeconds { get; init; }

    [JsonPropertyName("readSeconds")]
    public int ReadSeconds { get; init; }
}

public sealed class AuthConfig
{
    [JsonPropertyName("forwardHeaders")]
    public IReadOnlyList<string> ForwardHeaders { get; init; } = Array.Empty<string>();
}
