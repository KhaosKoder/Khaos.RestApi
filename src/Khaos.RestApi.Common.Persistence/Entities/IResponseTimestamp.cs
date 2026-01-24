namespace Common.Persistence.Entities;

internal interface IResponseTimestamp
{
    DateTimeOffset ResponseTimestampUtc { get; }
}