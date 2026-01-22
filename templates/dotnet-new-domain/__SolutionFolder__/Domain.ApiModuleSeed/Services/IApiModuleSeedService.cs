namespace Domain.ApiModuleSeed.Services;

public interface IApiModuleSeedService
{
    Task<string> PingAsync(CancellationToken cancellationToken = default);
}
