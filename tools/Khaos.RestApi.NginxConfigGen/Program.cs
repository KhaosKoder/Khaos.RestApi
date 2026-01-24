using System.Text;
using System.Text.Json;
using NginxConfigGen;

var configPath = GetArgumentValue(args, "--config", "-c")
    ?? Path.Combine(AppContext.BaseDirectory, "config", "apis.json");

var outputPath = GetArgumentValue(args, "--output", "-o")
    ?? Path.Combine(AppContext.BaseDirectory, "..", "..", "artifacts", "nginx");

var configFile = new FileInfo(configPath);
if (!configFile.Exists)
{
    Console.Error.WriteLine($"Config file not found: {configFile.FullName}");
    return 1;
}

var json = await File.ReadAllTextAsync(configFile.FullName).ConfigureAwait(false);
var config = JsonSerializer.Deserialize<NginxConfigRoot>(json, new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true
});

if (config is null || config.Apis.Count == 0)
{
    Console.Error.WriteLine("No APIs defined in config.");
    return 1;
}

var outputDir = new DirectoryInfo(outputPath);
if (!outputDir.Exists)
{
    outputDir.Create();
}

foreach (var api in config.Apis)
{
    var content = Render(api);
    var fileName = $"{api.Name.ToLowerInvariant()}.conf";
    var path = Path.Combine(outputDir.FullName, fileName);
    await File.WriteAllTextAsync(path, content, Encoding.UTF8).ConfigureAwait(false);
    Console.WriteLine($"Wrote {path}");
}

return 0;

static string? GetArgumentValue(string[] args, string longName, string shortName)
{
    for (var i = 0; i < args.Length; i++)
    {
        if (string.Equals(args[i], longName, StringComparison.OrdinalIgnoreCase) ||
            string.Equals(args[i], shortName, StringComparison.OrdinalIgnoreCase))
        {
            if (i + 1 < args.Length)
            {
                return args[i + 1];
            }
        }
    }

    return null;
}

static string Render(ApiConfig api)
{
    var nameSlug = api.Name.ToLowerInvariant();
    var upstreamName = $"{nameSlug}_upstream";
    var rateZone = $"{nameSlug}_ratelimit";

    var builder = new StringBuilder();
    builder.AppendLine($"upstream {upstreamName} {{");
    builder.AppendLine($"    server {api.Upstream.ServiceName}:{api.Upstream.Port};");
    builder.AppendLine("}\n");

    if (api.RateLimit.Enabled)
    {
        builder.AppendLine($"limit_req_zone $binary_remote_addr zone={rateZone}:10m rate={api.RateLimit.RequestsPerMinute}r/m;\n");
    }

    builder.AppendLine("server {");
    builder.AppendLine("    listen 80;");
    builder.AppendLine("    server_name gateway.local;\n");

    builder.AppendLine($"    location {api.PathPrefix}/ {{");
    builder.AppendLine($"        proxy_pass         http://{upstreamName};");
    builder.AppendLine("        proxy_set_header   Host $host;");
    builder.AppendLine("        proxy_set_header   X-Real-IP $remote_addr;");
    builder.AppendLine("        proxy_set_header   X-Forwarded-For $proxy_add_x_forwarded_for;");
    builder.AppendLine("        proxy_set_header   X-Forwarded-Proto $scheme;");
    builder.AppendLine("        proxy_set_header   X-Correlation-Id $request_id;\n");

    builder.AppendLine($"        proxy_connect_timeout   {api.Timeouts.ConnectSeconds}s;");
    builder.AppendLine($"        proxy_send_timeout      {api.Timeouts.SendSeconds}s;");
    builder.AppendLine($"        proxy_read_timeout      {api.Timeouts.ReadSeconds}s;\n");

    if (api.Auth.ForwardHeaders.Count > 0)
    {
        foreach (var header in api.Auth.ForwardHeaders)
        {
            var varName = header.ToLowerInvariant().Replace("-", "_");
            builder.AppendLine($"        proxy_set_header   {header} $http_{varName};");
        }
        builder.AppendLine();
    }

    if (api.RateLimit.Enabled)
    {
        builder.AppendLine($"        limit_req zone={rateZone} burst=10 nodelay;");
    }

    builder.AppendLine("    }");
    builder.AppendLine("}\n");

    return builder.ToString();
}
