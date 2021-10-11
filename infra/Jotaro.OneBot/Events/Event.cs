using System.Text.Json.Serialization;
using Jotaro.Utils.Converters;

namespace Jotaro.OneBot.Events;

[JsonConverter(typeof(JsonEventConverter))]
public abstract record Event([property: JsonIgnore] EventSet Set,
    [property: JsonPropertyName("sub_type")] string SubType, [property: JsonPropertyName("uuid")] string Uuid,
    [property: JsonPropertyName("self_id")] string SelfId, [property: JsonPropertyName("platform")] string Platform,
    [property: JsonConverter(typeof(JsonUnixTimestampConverter)), JsonPropertyName("time")] DateTimeOffset Time)
{
    internal const string TypePropertyName = "type";
    internal const string DetailTypePropertyName = "detail_type";

    [JsonPropertyName(TypePropertyName)]
    public EventType Type => Set.Type;

    [JsonPropertyName(DetailTypePropertyName)]
    public string DetailType => Set.DetailType;

    [JsonExtensionData]
    public Dictionary<string, object> ExtensionData { get; init; } = new();
}
