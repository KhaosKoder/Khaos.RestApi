using System.ComponentModel.DataAnnotations;

namespace Khaos.RestApi.Domain.ApiModuleSeed.Options;

/// <summary>
/// Configures Domain-layer defaults such as auditing, schema, and table layout.
/// </summary>
internal sealed class ApiModuleSeedDomainOptions
{
    public const string ConfigurationSection = "Domains:" + Khaos.RestApi.External.ApiModuleSeed.ApiModuleSeedConstants.ApiName;

    [Required]
    [MinLength(3)]
    public string DbSchema { get; init; } = "{{DB_SCHEMA}}";

    [Required]
    [MinLength(3)]
    public string TableMode { get; init; } = "{{TABLE_MODE}}";

    public bool EnableAuditing { get; init; } = bool.Parse("{{ENABLE_AUDIT}}");
}
