using Khaos.RestApi.Client.ApiModuleSeed.Models;

namespace Khaos.RestApi.Client.ApiModuleSeed.Services;

public interface IApiModuleSeedClient
{
    Task<PingResult> PingAsync(CancellationToken cancellationToken = default);
}
