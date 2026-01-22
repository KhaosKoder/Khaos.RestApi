using Client.ApiModuleSeed.Models;

namespace Client.ApiModuleSeed.Services;

public interface IApiModuleSeedClient
{
    Task<PingResult> PingAsync(CancellationToken cancellationToken = default);
}
