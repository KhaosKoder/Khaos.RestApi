namespace Client.ApiModuleSeed.Services;

public sealed class ApiModuleSeedClientOptions
{
    public const string ConfigurationSection = "ApiModuleSeed";

    public string BaseUrl { get; init; } = string.Empty;
}
