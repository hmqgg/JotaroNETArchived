using System.Text.Json.Serialization;

namespace Jotaro.OneBot.Events;

public record EventMeta(string SubType, string Uuid, string SelfId, string Platform, DateTimeOffset Time) : Event(
    EventSet.Meta, SubType, Uuid, SelfId, Platform, Time)
{
    internal const string EventTypeName = "meta";
}

// TODO: Implement `Status` with `get_status` Action.
public record EventMetaHeartbeat([property: JsonPropertyName("interval")] long Interval,
    [property: JsonPropertyName("status")] long Status, string SubType, string Uuid, string SelfId, string Platform,
    DateTimeOffset Time) : Event(EventSet.MetaHeartbeat, SubType, Uuid, SelfId, Platform, Time);
