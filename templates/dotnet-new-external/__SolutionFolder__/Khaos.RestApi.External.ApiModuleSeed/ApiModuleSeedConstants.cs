namespace Khaos.RestApi.External.ApiModuleSeed;

/// <summary>
/// Centralizes names that must stay consistent between layers.
/// </summary>
internal static class ApiModuleSeedConstants
{
    public const string ApiName = "ApiModuleSeed";
    public const string ConfigurationSection = "Apis:" + ApiName;
    public const string HttpClientName = ApiName + "External";
}
