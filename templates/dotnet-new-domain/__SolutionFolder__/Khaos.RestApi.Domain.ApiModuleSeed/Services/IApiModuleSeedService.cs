namespace Khaos.RestApi.Domain.ApiModuleSeed.Services;

public interface IApiModuleSeedService
{
    Task<string> PingAsync(CancellationToken cancellationToken = default);
}
