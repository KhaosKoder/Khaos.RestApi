namespace Internal.ApiModuleSeed.Responses;

public sealed class PingResponse
{
    public PingResponse(string echo)
    {
        Echo = echo;
    }

    public string Echo { get; }
}
